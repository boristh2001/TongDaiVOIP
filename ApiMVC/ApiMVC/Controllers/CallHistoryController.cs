using ApiMVC.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ApiMVC.Controllers
{
    public class CallHistoryController : Controller
    {
        // Lấy lịch sử cuộc gọi và lưu vào database
        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("http://dial.voip24h.vn/dial/history?voip=76af0a0d5f8445fa649525123d713c6bc2b2f9b8&secret=1366b46c23edb28f61aeae42fd571e00");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonConvert.DeserializeObject<ApiResult>(json);
                    var jsonData = jsonObject.Result.Data;
                    using (var db = new CallDbContext())
                    {
                        //var lastestCallDate = db.CallsHistory.OrderByDescending(x => x.CallDate).Select(x => x.CallDate).FirstOrDefault();
                        foreach (var item in jsonData)
                        {
                            var entity = new CallHistory
                            {
                                Id = item.Id,
                                CallDate = item.CallDate,
                                CallId = item.CallId,
                                Recording = item.Recording,
                                Play = item.Play,
                                Eplay = item.Eplay,
                                Download = item.Download,
                                Did = item.Did,
                                Src = item.Src,
                                Dst = item.Dst,
                                Status = item.Status,
                                Note = item.Note,
                                Disposition = item.Disposition,
                                LastApp = item.LastApp,
                                BillSec = item.BillSec,
                                Duration = item.Duration,
                                Type = item.Type,
                                Duration_Minutes = item.Duration_Minutes,
                                Duration_Seconds = item.Duration_Seconds
                            };

                            //Kiểm tra xem lịch sử cuộc gọi có tồn tại trong database hay không dựa theo callid
                            var existingData = db.CallsHistory.FirstOrDefault(x => x.CallId == entity.CallId);

                            //Nếu không tồn tại thì thêm mới
                            if (existingData == null /*&& entity.CallDate > lastestCallDate*/)
                            {
                               
                                db.CallsHistory.Add(entity);
                            }
                        }
                        await db.SaveChangesAsync();
                    }
                    return View(jsonData);
                }
                else
                {
                    return View("Error");
                }
            }
        }

        // Lấy lịch sử cuộc gọi nhưng không lưu vào database
        public async Task<ActionResult> GetHistoryCall()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://dial.voip24h.vn/dial/history?voip=76af0a0d5f8445fa649525123d713c6bc2b2f9b8&secret=1366b46c23edb28f61aeae42fd571e00");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResult>(responseContent);
                    var apiData = apiResponse.Result.Data;
                    return View(apiData);
                }
                else
                {
                    return View("Error");
                }
            }
        }

        // Tải xuống tập tin ghi âm và lưu vào thư mục Recordings
        public ActionResult DownloadForUrl(string downloadUrl)
        {
            using (WebClient client = new WebClient())
            {
                // Tải về tập tin từ URL của API
                byte[] result = client.DownloadData(downloadUrl);
                int startIndex = downloadUrl.IndexOf("&pkeyID=") + "&pkeyID=".Length;
                int endIndex = downloadUrl.LastIndexOf(".gsm");

                //Tạo đường dẫn để lưu tệp mới trong thư mục "Recordings" theo năm, tháng, ngày 
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString().PadLeft(2,'0');
                string recordingsPath = Server.MapPath("~/Recordings");
                string folderPath = Path.Combine(recordingsPath, year, month);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Tạo tên tệp mới cú pháp là ngày/tháng/năm để lưu tập tin vào thư mục "Downloads" của ứng dụng.
                string fileName = downloadUrl.Substring(startIndex, endIndex - startIndex) + ".wav";

                // Lưu tập tin vừa tải về vào đường dẫn chỉ định.
                System.IO.File.WriteAllBytes(Path.Combine(folderPath, fileName), result);

                // Trả về file vừa tải về để tải xuống và lưu vào thư mục Downloads.
                return File(result, "audio/wav", fileName);
            }
        }

        // Tạo thư mục theo cấu trúc Recordings/Năm/Tháng/Tên file
        public string SaveFileUrl(byte[] data, string fileName)
        {
            string recordingPath = Server.MapPath("~/Recordings");
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString().PadLeft(2,'0');
            string folderPath = Path.Combine(recordingPath, year, month);

            if(!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            System.IO.File.WriteAllBytes(Path.Combine(folderPath, fileName),data);
            return Path.Combine(folderPath, fileName);
        }

        // Xử lý đặt tên file và lưu đường dẫn chứa tập tin vào database
        public async Task<ActionResult> DownloadFileUrl(string downloadUrl)
        {
            using (WebClient web = new WebClient())
            {
                try
                {
                    byte[] result = web.DownloadData(downloadUrl);
                    int startIndex = downloadUrl.IndexOf("&pkeyID=") + "&pkeyID=".Length;
                    int lastIndex = downloadUrl.LastIndexOf(".gsm");
                    string fileName = downloadUrl.Substring(startIndex, lastIndex - startIndex) + ".wav";

                    var filePath = SaveFileUrl(result, fileName);
                    using(var db = new CallDbContext())
                    {
                        var callHistory = db.CallsHistory.FirstOrDefault(x => x.Download == downloadUrl);
                        if (callHistory != null)
                        {
                            callHistory.Download = filePath;
                        }
                        else
                        {
                            var entity = new CallHistory
                            {
                                Download = filePath
                            };
                            db.CallsHistory.Add(entity);
                        }
                        await db.SaveChangesAsync();
                    }
                    return File(result, "audio/wav", Path.GetFileName(filePath));
                }
                catch(Exception ex)
                {
                    return RedirectToAction("FailedResponse","Home");
                }
            }
        }
    }
}

$(function() {
	var url = '/CallApiMvc/Call';
	var user = JSON.parse(localStorage.getItem('SIPCreds'));
	if (user) {
		$.each(user, function (k, v) { $('input[name="' + k + '"]').val(v); });
		url = '/CallApiMvc/Call'; 
		window.location.replace(url);
	}


	$('#btnConfig').click(function (event) {
		user = {}, valid = true;
		event.preventDefault();
		$('#mdlDemo input').each(function(i){if($(this).val() === '') {$(this).closest('.form-group').addClass('has-error'); valid = false;}else {$(this).closest('.form-group').removeClass('has-error');} user[$(this).attr('name')] = $(this).val(); });
		if (valid) {
			localStorage.setItem('SIPCreds', JSON.stringify(user)); 
			if (localStorage.getItem('SIPCreds')){ window.location.replace(url); }else { confirm('Phone Error Setting.');}
		}
	});


});
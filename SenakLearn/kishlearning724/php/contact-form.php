<?php
if(isset($_POST["action"])) {
	$name = $_POST['name'];                 // نام فرستنده
	$email = $_POST['email'];     // ایمیل فرستنده
	$phone  = $_POST['phone'];     // شماره موبایل فرستنده
	$website  = $_POST['website'];     // سایت فرستنده
	$message = $_POST['message'];    // پیام فرستنده
	$from = 'Demo Contact Form';    
	$to = 'Demo@domian.com';     // ایمیل خود را به جای ایمیل رو به رو جایگزین کنید
	$subject = 'Message from Contact Demo ';

	//$body = " از طرف: $name \n ایمیل: $email \n شماره موبایل : $phone \n پیام : $message"  ;
	$body = "از طرف: $name \n";   
  	$body.= "آدرس ایمیل: $email \n";
	$body.= "شماره موبایل : $phone \n";  
	$body.= "سایت : $website \n";  
	$body.= "پیام : $message \n";
	
	// init error message 
	$errmsg='';
	// Check if name has been entered
	if (!$_POST['name']) {
		$errmsg .= 'لطفا نام خود را وارد کنید'."<br>";
	}

	
	/* Check required field not blank */
	
	// Check if email has been entered and is valid
	if (!$_POST['email'] || !filter_var($_POST['email'], FILTER_VALIDATE_EMAIL)) {
		$errmsg .= 'لطفا یک آدرس ایمیل معتبر وارد کنید'."<br>";
	}	

	//Check if message has been entered
	if (!$_POST['message']) {
		$errmsg .= 'لطفا پیام خود را بنویسید'."<br>";
	}
 
	$result='';
	// If there are no errors, send the email
	if (!$errmsg) {
		if (mail ($to, $subject, $body, $from)) {
			$result='<div class="alert alert-success">از تماس شما متشکریم . ایمیل شما دریافت شد و به زودی با شما تماس خواهیم گرفت</div>'; 
		} 
		else {
		  $result='<div class="alert alert-danger">ببخشید اما به نظر میاد خطایی رخ داده است. لطفا بعدا دوباره امتحان کنید</div>';
		}
	}
	else{
		$result='<div class="alert alert-danger">'.$errmsg.'</div>';
	}
		echo $result;
	}
?>

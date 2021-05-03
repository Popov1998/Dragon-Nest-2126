<?php
    require_once "config.php";

    if(isset($_SESSION['AccountID'])){
        echo "
        <li><a id='account-btn' class='point'>Account</a></li>
        <li><a id='download-btn' class='point'>Download</a></li>
        ";
    } else {
        echo "
        <li><a id='register-btn' class='point'>Register</a></li>
        <li><a id='login-btn' class='point'>Login</a></li>
        <li><a id='download-btn' class='point'>Download</a></li>
        ";
    }
?>
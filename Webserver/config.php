<?php
	$serverName  = "127.0.0.1, 1433";
	$connectionInfo  = array("UID"=>"DragonNest", "PWD"=>"skQmsgozj!*sha");
	$conn = sqlsrv_connect($serverName, $connectionInfo);

	if($conn === false){
		echo "Unable to Connect to the Server!";
	}
?>
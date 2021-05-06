<?php
error_reporting(0);
ini_set('display_errors',0);

function MakeResponse($status,$user,$balance){
    $arr["RESULT-CODE"] =$status;
	$arr["RESULT-MESSAGE"] ="Success";
	$arr["SID"] ="DRNEST";
	$arr["CN"] ="44820790";
	$arr["UID"] = $user;
	$arr["CASH-BALANCE"] = $balance;
	
	return json_encode($arr);
}

function GetBalance($user){

    $serverName  = "127.0.0.1, 1433";
    $connectionInfo  = array("Database"=>"DNMembership", "UID"=>"DragonNest", "PWD"=>"skQmsgozj!*sha");
    $conn = sqlsrv_connect($serverName, $connectionInfo);

    $sql = "EXEC [dbo].[__NX__GetBalance] ?, ?";
    $balance = 0;
    $i = array($user, array(SQLSRV_PARAM_IN));
    $b = array($balance, array(SQLSRV_PARAM_IN));
    $param = array($i, $b);
    
    // prepares executing
    $stmt = sqlsrv_prepare($conn, $sql, $param);

    // Execute the statement
    sqlsrv_execute($stmt);

    // Free SQL Statement
    sqlsrv_free_stmt( $stmt);
    return $balance;
}

function SetBalance($user,$amount){
    $serverName  = "127.0.0.1, 1433";
    $connectionInfo  = array("Database"=>"DNMembership", "UID"=>"DragonNest", "PWD"=>"skQmsgozj!*sha");
    $conn = sqlsrv_connect($serverName, $connectionInfo);

    $sql = "EXEC [dbo].[__NX__UpdateCashBalance] ?, ?, ?";
    $balance = 0;
    $i = array($user, array(SQLSRV_PARAM_IN));
    $a = array($amount, array(SQLSRV_PARAM_IN));
    $b = array($balance, array(SQLSRV_PARAM_IN));
    $param = array($i, $a, $b);
    
    // prepares executing
    $stmt = sqlsrv_prepare($conn, $sql, $param);

    // Execute the statement
    sqlsrv_execute($stmt);

    // Free SQL Statement
    sqlsrv_free_stmt( $stmt);
    return $balance;
}

$serverName  = "127.0.0.1, 1433";
$connectionInfo  = array("Database"=>"DNMembership", "UID"=>"DragonNest", "PWD"=>"skQmsgozj!*sha");
$conn = sqlsrv_connect($serverName, $connectionInfo);

$id = $_POST['BUYER-ID']; 
$total = $_POST['TOTAL-PRICE'];

if(isset($id) || isset($total)){
    $sql = "SELECT AccountID FROM dbo.Accounts WHERE AccountName = ?";
    $param = array($id);
    $options = array("Scrollable" => SQLSRV_CURSOR_KEYSET);
    $exists = sqlsrv_query($conn, $sql, $param, $options);

    if(sqlsrv_num_rows($exists) > 0){
        $balance = GetBalance($id);
        
        if($balance-$total < 0){
            echo MakeResponse("E402",$id,$balance);
        } else {
            $leftbalance = SetBalance($id,-$total);

            echo MakeResponse("S000",$id,$leftbalance);
        }
    } else {
        echo MakeResponse("E301",$id,0);
    }
}
?>
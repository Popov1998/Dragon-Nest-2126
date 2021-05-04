<?php
error_reporting(0);
ini_set('display_errors',0);
$serverName  = "127.0.0.1, 1433";
$connectionInfo  = array("Database"=>"DNMembership", "UID"=>"DragonNest", "PWD"=>"skQmsgozj!*sha");
$conn = sqlsrv_connect($serverName, $connectionInfo);

$id = $_POST['id'];
$password= $_POST['password'];

if(isset($id)){
    $sql = "SELECT AccountID FROM [dbo].[Accounts] WHERE AccountName = ?";
    $param = array($id);
    $options = array("Scrollable" => SQLSRV_CURSOR_KEYSET);
    $exists = sqlsrv_query($conn, $sql, $param, $options);

    if(sqlsrv_num_rows($exists) > 0){
        $sql = "SELECT AccountID FROM [dbo].[Accounts] WHERE AccountName = ? AND NxLoginPwd = ?";
        $param = array($id, $password);
        $options = array("Scrollable" => SQLSRV_CURSOR_KEYSET);
        $exists = sqlsrv_query($conn, $sql, $param, $options);

        if(sqlsrv_num_rows($exists) > 0){
            echo 'S000	OK	0';
        } else {
            echo 'E203	OK	0';
        }
    } else {
        echo 'E202	OK	0';
    }
} else {
    echo 'E205  OK 0';
}
?>
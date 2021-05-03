<?php
error_reporting(0);
ini_set('display_errors',0);
$serverName  = "127.0.0.1, 1433";
$connectionInfo  = array("Database"=>"DNMembership", "UID"=>"DragonNest", "PWD"=>"skQmsgozj!*sha");
$conn = sqlsrv_connect($serverName, $connectionInfo);

function MakeResponse($status,$user,$balance){
    $arr["RESULT-CODE"]=$status;
    $arr["RESULT-MESSAGE"]="Success";
    $arr["SID"]="DRNEST";
    $arr["CN"]="44820790";
    $arr["UID"]=$user;
    $arr["CASH-BALANCE"] = $balance;
    return json_encode($arr);
}

$id = $_POST['UID'];

if(isset($id)){
    $sql = "SELECT AccountID FROM [dbo].[Accounts] WHERE AccountName = ?";
    $param = array($id);
    $options = array("Scrollable" => SQLSRV_CURSOR_KEYSET);
    $exists = sqlsrv_query($conn, $sql, $param, $options);

    if(sqlsrv_num_rows($exists) > 0){
        $sql = "EXEC [dbo].[__GetBalance] ?, ?";
        $balance = 0;
        $i = array($id, array(SQLSRV_PARAM_IN));
        $b = array($balance, array(SQLSRV_PARAM_IN));
        $param = array($i, $b);
        
        // prepares executing
        $stmt = sqlsrv_prepare($conn, $sql, $param);

        // Execute the statement
        sqlsrv_execute($stmt);

        // Free SQL Statement
        sqlsrv_free_stmt( $stmt);

        echo MakeResponse("S000",$id,$balance);
    } else {
        echo MakeResponse("E301",$id,$balance);
    }
}
?>
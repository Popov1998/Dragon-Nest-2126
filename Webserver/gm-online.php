<?php
    require_once "config.php";

    $sql = "SELECT * FROM [DNMembership].[dbo].[DNAuth] WHERE CertifyingStep = ? AND AccountLevel = ?";
    $param = array(2, 99);
    $options =  array( "Scrollable" => SQLSRV_CURSOR_KEYSET );
    $stmt = sqlsrv_query( $conn, $sql , $param, $options );
    $online = sqlsrv_num_rows($stmt);

    echo $online;
?>
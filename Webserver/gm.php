<?php
    require_once "config.php";

    $sql = "SELECT * FROM [DNMembership].[dbo].[Accounts] WHERE AccountLevelCode = ?";
    $param = array(99);
    $options =  array( "Scrollable" => SQLSRV_CURSOR_KEYSET );
    $stmt = sqlsrv_query( $conn, $sql , $param, $options );
    $row_count = sqlsrv_num_rows($stmt);

    echo $row_count;
?>
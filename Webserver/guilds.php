<?php
    require_once "config.php";

    $sql = "SELECT * FROM [DNWorld].[dbo].[Guilds]";
    $param = array();
    $options =  array( "Scrollable" => SQLSRV_CURSOR_KEYSET );
    $stmt = sqlsrv_query( $conn, $sql , $param, $options );
    $row_count = sqlsrv_num_rows($stmt);

    echo $row_count;
?>
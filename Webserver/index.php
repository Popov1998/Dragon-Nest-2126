<?php 
    session_start();
?>
<!DOCTYPE html>
<html>
    <head>
        <title>Dragon Nest</title>
        <link rel="stylesheet" href="/library/css/style.css" type="text/css">
        <script src="//cdn.jsdelivr.net/npm/sweetalert2@10"></script>
    </head>
    <body>
        <header>
            <div class="container-header">
                <a class="navlink">Dragon Nest</a>
                <nav>
                    <ul>
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
                    </ul>
                </nav>
            </div>
        </header>
        <div id="register" class="modal-register" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <form action="" method="POST">
            <?php
                require_once "config.php";

                // if(!empty($_POST) && $_SERVER['REQUEST_METHOD'] == 'POST'){
                //     $data = array($_POST['mail_regis'], $_POST['name_regis'], $_POST['pass_regis'], $_POST['repass_regis']);
                //     unset($data);
                // }

                if(isset($_POST['submit'])){
                    if(empty($_POST['mail_regis']) || empty($_POST['name_regis']) || empty($_POST['pass_regis']) || empty($_POST['repass_regis'])){
                        $data = array($_POST['mail_regis'], $_POST['name_regis'], $_POST['pass_regis'], $_POST['repass_regis']); 
                        echo "
                        <script>
                            swal.fire({
                                title: 'REGISTRATION ERROR!',
                                text: 'Required Fields are empty!',
                                icon: 'warning',
                                allowOutsideClick: false
                            }).then(function () {
                                var modal = document.getElementById('register');
                                modal.style.display = 'block';
                            });
                        </script>
                        ";

                        unset($data);
                    } else {
                        if($_POST['pass_regis'] !== $_POST['repass_regis']){
                            echo "
                            <script>
                                swal.fire({
                                    title: 'REGISTRATION ERROR!',
                                    text: 'Password Didn`t Matched!',
                                    icon: 'warning',
                                    allowOutsideClick: false
                                }).then(function () {
                                    var modal = document.getElementById('register');
                                    modal.style.display = 'block';
                                });
                            </script>
                            ";

                            unset($data);
                        } else {
                            if(strlen($_POST['name_regis']) > 8){
                                echo "
                                <script>
                                    swal.fire({
                                        title: 'REGISTRATION ERROR!',
                                        text: 'Username is too long!',
                                        icon: 'warning',
                                        allowOutsideClick: false
                                    }).then(function () {
                                        var modal = document.getElementById('register');
                                        modal.style.display = 'block';
                                    });
                                </script>
                                ";

                                unset($data);
                            } else {
                                if(strlen($_POST['pass_regis']) < 4){
                                    echo "
                                    <script>
                                        swal.fire({
                                            title: 'REGISTRATION ERROR!',
                                            text: 'Password is too short!',
                                            icon: 'warning',
                                            allowOutsideClick: false
                                        }).then(function () {
                                            var modal = document.getElementById('register');
                                            modal.style.display = 'block';
                                        });
                                    </script>
                                    ";

                                    unset($data);
                                } else {
                                    if(strlen($_POST['pass_regis']) > 10){
                                        echo "
                                        <script>
                                            swal.fire({
                                                title: 'REGISTRATION ERROR!',
                                                text: 'Password is too long!',
                                                icon: 'warning',
                                                allowOutsideClick: false
                                            }).then(function () {
                                                var modal = document.getElementById('register');
                                                modal.style.display = 'block';
                                            });
                                        </script>
                                        ";

                                        unset($data);
                                    } else {
                                        $sql = "SELECT Mail FROM [DNMembership].[dbo].[Accounts] WHERE Mail = ?";
                                        $param = array($_POST['mail_regis']);
                                        $options = array("Scrollable" => SQLSRV_CURSOR_KEYSET);
                                        $execute = sqlsrv_query($conn, $sql, $param, $options);
                                        $stmt = sqlsrv_num_rows($execute);

                                        if($stmt > 0){
                                            echo "
                                            <script>
                                                swal.fire({
                                                    title: 'REGISTRATION ERROR!',
                                                    text: '" . $_POST['mail_regis'] . " is already used!',
                                                    icon: 'warning',
                                                    allowOutsideClick: false
                                                }).then(function () {
                                                    var modal = document.getElementById('register');
                                                    modal.style.display = 'block';
                                                });
                                            </script>
                                            ";

                                            unset($data);
                                        } else {
                                            $sql = "SELECT AccountName FROM [DNMembership].[dbo].[Accounts] WHERE AccountName = ?";
                                            $param = array($_POST['name_regis']);
                                            $options = array("Scrollable" => SQLSRV_CURSOR_KEYSET);
                                            $execute = sqlsrv_query($conn, $sql, $param, $options);
                                            $stmt = sqlsrv_num_rows($execute);

                                            if($stmt > 0){
                                                echo "
                                                <script>
                                                    swal.fire({
                                                        title: 'REGISTRATION ERROR!',
                                                        text: '" . $_POST['name_regis'] . " is already used!',
                                                        icon: 'warning',
                                                        allowOutsideClick: false
                                                    }).then(function () {
                                                        var modal = document.getElementById('register');
                                                        modal.style.display = 'block';
                                                    });
                                                </script>
                                                ";

                                                unset($data);
                                            } else {
                                                $sql = "EXEC [DNMembership].[dbo].[__NX__CreateAccount] ?, ?, ?";
                                                $param = array($_POST['name_regis'], $_POST['pass_regis'], $_POST['mail_regis']);
                                                $execute = sqlsrv_query($conn, $sql, $param);

                                                $cash = "EXEC [DNMembership].[dbo].[P_AddCashIncome] ?, ?, ?, ?, ?";
                                                $cparam = array($_POST['name_regis'], 0, 0, 0, 500000);
                                                $execute = sqlsrv_query($conn, $cash, $cparam);

                                                if($execute === false){
                                                    echo "
                                                    <script>
                                                        swal.fire({
                                                            title: 'REGISTRATION ERROR!',
                                                            text: 'Unable to Submit Data!',
                                                            icon: 'warning',
                                                            allowOutsideClick: false
                                                        }).then(function () {
                                                            var modal = document.getElementById('register');
                                                            modal.style.display = 'block';
                                                        });
                                                    </script>
                                                    ";

                                                    unset($data);
                                                } else {
                                                    echo "
                                                    <script>
                                                        swal.fire({
                                                            title: 'REGISTRATION SUCCESS!',
                                                            icon: 'success',
                                                            allowOutsideClick: false
                                                        }).then(function () {
                                                            location.href='http://127.0.0.1/';
                                                        });
                                                    </script>
                                                    ";

                                                    unset($data);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            ?>
                <div class="modal-content-register">
                    <span class="close">&times;</span>
	                <div style="border-bottom: 2px solid #1d2333">
                        <p class="info info-none center">REGISTRATION</p>
                    </div>
                    <div class="modal-body-register">
                            <input type="email" name="mail_regis" placeholder="EMAIL" >
                            <input type="text" name="name_regis" placeholder="USERNAME" >
                            <input type="password" name="pass_regis" placeholder="PASSWORD" >
                            <input type="password" name="repass_regis" placeholder="RE-TYPE PASSWORD" >
                            <center><button type="submit" name="submit">SUBMIT</button></center>
                    </div>
                </div>
            </form>
        </div>
        <div id="login" class="modal-login" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <form action="" method="POST">
            <?php
                require_once "config.php";

                if(isset($_POST['login'])){
                    if(empty($_POST['name_login']) || empty($_POST['pass_login'])){
                        echo "
                        <script>
                            swal.fire({
                                title: 'LOGIN ERROR!',
                                text: 'Required Fields are empty!',
                                icon: 'warning',
                                allowOutsideClick: false
                            }).then(function () {
                                var modal = document.getElementById('login');
                                modal.style.display = 'block';
                            });
                        </script>
                        ";
                    } else {
                        $passmd5 = md5($_POST['pass_login']);
                        $sql = "SELECT * FROM [DNMembership].[dbo].[Accounts] WHERE AccountName=? AND NxLoginPwd=?";
                        $param = array($_POST['name_login'], $passmd5);
                        $result = sqlsrv_query($conn, $sql, $param);

                        if(sqlsrv_has_rows($result) <= 0){
                            echo "
                            <script>
                                swal.fire({
                                    title: 'LOGIN ERROR!',
                                    text: 'Incorrect Username or Password!',
                                    icon: 'warning',
                                    allowOutsideClick: false
                                }).then(function () {
                                    var modal = document.getElementById('login');
                                    modal.style.display = 'block';
                                });
                            </script>
                            ";
                        } else {
                            while($row = sqlsrv_fetch_array($result)){
                                $_SESSION['AccountID'] = $row['AccountID'];
                                $_SESSION['AccountName'] = $row['AccountName'];

                                echo "
                                <script>
                                    swal.fire({
                                        title: 'LOGIN SUCCESS!',
                                        icon: 'success',
                                        allowOutsideClick: false
                                    }).then(function () {
                                        location.href='http://127.0.0.1/';
                                    });
                                </script>
                                ";
                            }
                        }
                    }
                }
            ?>
                <div class="modal-content-login">
                    <span class="close-login">&times;</span>
	                <div style="border-bottom: 2px solid #1d2333">
                        <p class="info info-none center">LOGIN</p>
                    </div>
                    <div class="modal-body-login">
                            <input type="text" name="name_login" placeholder="USERNAME" >
                            <input type="password" name="pass_login" placeholder="PASSWORD" >
                            <center><button type="submit" name="login">SUBMIT</button></center>
                    </div>
                </div>
            </form>
        </div>
        <div id="download" class="modal-download" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                <div class="modal-content-download">
                    <span class="close-download">&times;</span>
	                <div style="border-bottom: 2px solid #1d2333">
                        <p class="info info-none center">DOWNLOADS</p>
                    </div>
                    <div class="modal-body-download">
                    </div>
                </div>
        </div>
        <div id="account" class="modal-account" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                <div class="modal-content-account">
                    <span class="close-account">&times;</span>
	                <div style="border-bottom: 2px solid #1d2333">
                        <p class="info info-none center"><?php 
                        if(isset($_SESSION['AccountID'])){
                            echo $_SESSION['AccountName']; 
                        }
                        ?></p>
                    </div>
                    <div class="modal-body-account">
                        <p class="info info-none">CASH:  <?php 
                        require_once "config.php";

                        if(isset($_SESSION['AccountID'])){
                            $sql = "{call DNMembership.dbo.__NX__GetBalance_ID (?, ?)}";
                            $param = array($_SESSION['AccountID'], 0);
                            $stmt = sqlsrv_query($conn, $sql, $param);
                            
                            if( sqlsrv_fetch( $stmt ) === false) {
                                die( print_r( sqlsrv_errors(), true));
                           } else {
                               $result = sqlsrv_get_field($stmt, 0);
                               echo "$result";
                           }
                        }
                        ?></p>
                        <p class="info info-none">CHARACTER COUNT:  <?php
                        require_once "config.php";
                        
                        if(isset($_SESSION['AccountID'])){
                            $sql = "SELECT * FROM [DNWorld].[dbo].[Characters] WHERE AccountID = ? AND AccountName = ?";
                            $param = array($_SESSION['AccountID'], $_SESSION['AccountName']);
                            $option = array("Scrollable" => SQLSRV_CURSOR_KEYSET);
                            $stmt = sqlsrv_query($conn, $sql, $param, $option);
                            $characters = sqlsrv_num_rows($stmt);

                            echo $characters;
                        }
                        ?></p>
                        <p class="info info-none">ACCOUNT STATUS:  <?php
                            require_once "config.php";

                            if(isset($_SESSION['AccountID'])){
                                $sql = "SELECT AccountID, EnglishName FROM [DNMembership].[dbo].[Accounts], [DNMembership].[dbo].[AccountStatus] WHERE AccountID = ? AND AccountLevel = AccountLevelCode";
                                $param = array($_SESSION['AccountID']);
                                $option = array("Scrollable" => SQLSRV_CURSOR_KEYSET);
                                $stmt = sqlsrv_query($conn, $sql, $param, $option);

                                if(sqlsrv_num_rows($stmt) > 0){
                                    while($row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC)){
                                        echo $row['EnglishName'];
                                    }
                                }
                            }
                        ?></p>
                    </div>
                    <div class="modal-account-footer">
                        <?php
                            if(isset($_POST['logout'])){
                                session_destroy();
                                echo "
                                <script>
                                    swal.fire({
                                        title: 'LOGOUT SUCCESS!',
                                        icon: 'success',
                                        allowOutsideClick: false
                                    }).then(function () {
                                        location.href='http://127.0.0.1/';
                                    });
                                </script>
                                ";
                            }
                        ?>
                        <form action="" method="POST">
                            <center><button type="submit" name="logout">LOGOUT</button></center>
                        </form>
                    </div>
                </div>
        </div>
        <div class="content">
            <center><p class="info info-none">NA NA NA</p></center>
        </div>
        <div class="container">
            <p class="info">Online Players: <?php 
                require_once "config.php";

                $sql = "SELECT * FROM [DNMembership].[dbo].[DNAuth] WHERE CertifyingStep = ?";
                $param = array(2);
                $options =  array( "Scrollable" => SQLSRV_CURSOR_KEYSET );
                $stmt = sqlsrv_query( $conn, $sql , $param, $options );
                $online = sqlsrv_num_rows($stmt);
            
                echo $online;
            ?></p>
            <p class="info">Online Game Master: <?php 
                require_once "config.php";

                $sql = "SELECT * FROM [DNMembership].[dbo].[DNAuth] WHERE CertifyingStep = ? AND AccountLevel = ?";
                $param = array(2, 99);
                $options =  array( "Scrollable" => SQLSRV_CURSOR_KEYSET );
                $stmt = sqlsrv_query( $conn, $sql , $param, $options );
                $online = sqlsrv_num_rows($stmt);
            
                echo $online;
            ?></p>
            <p class="info">Accounts: <?php 
                require_once "config.php";

                $sql = "SELECT * FROM [DNMembership].[dbo].[Accounts]";
                $param = array();
                $options =  array( "Scrollable" => SQLSRV_CURSOR_KEYSET );
                $stmt = sqlsrv_query( $conn, $sql , $param, $options );
                $row_count = sqlsrv_num_rows($stmt);
            
                echo $row_count;
            ?></p>
            <p class="info">Game Master: <?php 
                require_once "config.php";

                $sql = "SELECT * FROM [DNMembership].[dbo].[Accounts] WHERE AccountLevelCode = ?";
                $param = array(99);
                $options =  array( "Scrollable" => SQLSRV_CURSOR_KEYSET );
                $stmt = sqlsrv_query( $conn, $sql , $param, $options );
                $row_count = sqlsrv_num_rows($stmt);
            
                echo $row_count;
            ?></p>
            <p class="info">Guilds: <?php 
                require_once "config.php";

                $sql = "SELECT * FROM [DNWorld].[dbo].[Guilds]";
                $param = array();
                $options =  array( "Scrollable" => SQLSRV_CURSOR_KEYSET );
                $stmt = sqlsrv_query( $conn, $sql , $param, $options );
                $row_count = sqlsrv_num_rows($stmt);
            
                echo $row_count;
            ?></p>
            <p class="info">Characters: <?php 
                require_once "config.php";

                $sql = "SELECT * FROM [DNWorld].[dbo].[Characters]";
                $param = array();
                $options =  array( "Scrollable" => SQLSRV_CURSOR_KEYSET );
                $stmt = sqlsrv_query( $conn, $sql , $param, $options );
                $row_count = sqlsrv_num_rows($stmt);
            
                echo $row_count;
            ?></p>
            <p class="info">Server Time: <?php echo date('h:i A', time()); ?></p>
            <p class="info info-none">IP Address: <!--<script type="application/javascript">
                                function getIP(json) {
                                    document.write(json.ip);
                                }
                            </script>
            <script type="application/javascript" src="https://api.ipify.org?format=jsonp&callback=getIP"></script>--></p>
        </div>
        <footer class="foot">
            <p class="foot-paragraph">©<?php echo date("Y"); ?> Adventure Games LLC Inc.</p>
        </footer>
        <script>
            // ACCOUNT
            if(document.getElementById("account-btn")){
                document.getElementById("account-btn").onclick = function(){
                    document.getElementById('account').style.display = "block";
                }

                document.getElementsByClassName("close-account")[0].onclick = function(){
                    document.getElementById('account').style.display = "none";
                }

                window.onclick = function(event){
                    if (event.target == document.getElementById('account')){
                        modal.style.display = "none";
                    }
                }
            } else {
                null
            }
            // DOWNLOAD
            if(document.getElementById("download-btn")){
                document.getElementById("download-btn").onclick = function(){
                    document.getElementById('download').style.display = "block";
                }

                document.getElementsByClassName("close-download")[0].onclick = function(){
                    document.getElementById('download').style.display = "none";
                }

                window.onclick = function(event){
                    if (event.target == document.getElementById('download')){
                        modal.style.display = "none";
                    }
                }
            } else {
                null
            }
            //REGISTER
            if(document.getElementById("register-btn")){
                document.getElementById("register-btn").onclick = function(){
                    document.getElementById('register').style.display = "block";
                }

                document.getElementsByClassName("close")[0].onclick = function(){
                    document.getElementById('register').style.display = "none";
                }

                window.onclick = function(event){
                    if (event.target == document.getElementById('register')){
                        modal.style.display = "none";
                    }
                }
            } else {
                null
            }
            //LOGIN
            if(document.getElementById("login-btn")){
                document.getElementById("login-btn").onclick = function(){
                    document.getElementById('login').style.display = "block";
                }

                document.getElementsByClassName("close-login")[0].onclick = function(){
                    document.getElementById('login').style.display = "none";
                }

                window.onclick = function(event){
                    if (event.target == document.getElementById('login')){
                        modal.style.display = "none";
                    }
                }
            } else {
                null
            }
        </script>
    </body>
</html>
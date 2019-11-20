<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMensajePermisos.aspx.cs" Inherits="Solution_CTT.frmMensajePermisos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <title>Advertencia</title>
    <link rel="icon" type="image/png" href="../assets/img/favicon_exp_ate.png" />
	<link rel="stylesheet" type="text/css" href="assets/bootstrap/css/bootstrap.min.css"/>
	<link rel="stylesheet" type="text/css" href="assets/css/my-login.css"/>  
    <%--sweet alert--%>
    <link href="assets/css/sweetalert.css" rel="stylesheet" />
    <script src="assets/js/sweetalert.min.js"></script>    
    <%--ANIMACION--%>
    <style type="text/css">
	@keyframes animatedBackground {
		from { background-position: 0 0; }
		to { background-position: 100% 0; }
	}
	@-webkit-keyframes animatedBackground {
		from { background-position: 0 0; }
		to { background-position: 100% 0; }
	}
	@-ms-keyframes animatedBackground {
		from { background-position: 0 0; }
		to { background-position: 100% 0; }
	}
	@-moz-keyframes animatedBackground {
		from { background-position: 0 0; }
		to { background-position: 100% 0; }
	}

	#animate-area	{ 
		background-image: url(assets/img/cloud.png);
		background-position: 0px 0px;
		background-repeat: repeat-x;

		animation: animatedBackground 40s linear infinite;
		-ms-animation: animatedBackground 40s linear infinite;
		-moz-animation: animatedBackground 40s linear infinite;
		-webkit-animation: animatedBackground 40s linear infinite;
	}
</style>
</head>
<body class="my-login-page" id="animate-area">
	<section class="h-100" id="animationSandbox">
		<div class="container h-100">
			<div class="row justify-content-md-center h-100">
				<div class="card-wrapper">
					<div class="brand">
						<img src="assets/img/logo.jpg"/>
					</div>
					<div class="card fat">
						<div class="card-body">
							<h4 class="card-title">Aviso</h4>
							<form id="form1" runat="server">							 
                                <div class="form-group">
									<label for="email">No tiene permisos para acceder a esta sección.</label>
								</div>
                              
								<div class="form-group no-margin">
                                    <asp:Button ID="btnRegresar" runat="server" Text="Volver al Menú Principal" class="btn btn-primary btn-block" TabIndex="4" OnClick="btnRegresar_Click"/>                                    
								</div>
							</form>
						</div>
					</div>
					<div class="footer">
						
					</div>
				</div>
			</div>
		</div>
	</section>
	<script src="assets/js/jquery.min.js"></script>
	<script src="assets/bootstrap/js/bootstrap.min.js"></script>
	<script src="assets/js/my-login.js"></script>
</body>
</html>

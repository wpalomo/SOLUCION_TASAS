<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLogin.aspx.cs" Inherits="Solution_CTT.frmLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <title>Ingresar al Sistema</title>
    <link rel="icon" type="image/png" href="../assets/img/favicon_exp_ate.png" />
	<link rel="stylesheet" type="text/css" href="assets/bootstrap/css/bootstrap.min.css"/>
	<link rel="stylesheet" type="text/css" href="assets/css/my-login.css"/>  
    <%--sweet alert--%>
    <link href="assets/css/sweetalert.css" rel="stylesheet" />
    <script src="assets/js/sweetalert.min.js"></script>
    
</head>
<body class="my-login-page" id="animate-area">
	<section class="h-100" id="animationSandbox">
		<div class="container h-100">
			<div class="row justify-content-md-center h-100">
				<div class="card-wrapper">
					<div class="brand">
						
					</div>
					<div class="card fat">
						<div class="card-body">
							<h4 class="card-title">Login</h4>
							<form id="form1" runat="server">							 
                                <div class="form-group">
									<%--<label for="email"><%= Resources.MESSAGES.TXT_SUCURSAL %></label>
                                    <asp:DropDownList ID="cmbTerminal" class="form-control" runat="server"></asp:DropDownList>--%>
                                    <h2><b><asp:Label ID="lblSucursal" runat="server" Text=""></asp:Label></b></h2>
								</div>
								<div class="form-group">
									<label for="email"><%= Resources.MESSAGES.TXT_USER %></label>
                                    <asp:TextBox ID="txtUsuario" runat="server" required="" autocomplete="off" Text="" class="form-control" placeholder="USUARIO" TabIndex="1"></asp:TextBox>									
								</div>
								<div class="form-group">
									<label for="password"><%= Resources.MESSAGES.TXT_PASS %>
										
									</label>
                                    <asp:TextBox ID="txtPassword" runat="server" required="" class="form-control" TabIndex="2" TextMode="Password" placeholder="CONTRASEÑA"></asp:TextBox>
								</div>

								<%--<asp:RadioButton ID="rdbMatutina" runat="server" Text="&nbsp&nbspMAÑANA" Checked="true" GroupName="jornada" />
                                <asp:RadioButton ID="rdbVespertina" runat="server" Text="&nbsp&nbspTARDE" GroupName="jornada" />--%>
								
                                <div class="form-group no-margin">
                                    <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" class="btn btn-primary btn-block" TabIndex="4" OnClick="btnIngresar_Click"/>                                    
								</div>
							</form>
						</div>
					</div>
				</div>
			</div>
		</div>
	</section>

	<script src="assets/js/jquery.min.js"></script>
	<script src="assets/bootstrap/js/bootstrap.min.js"></script>

    <script src="assets/js/jquery.backstretch.js"></script>
    <script>
        $.backstretch([
          "assets/img/AtenasBack1.jpg",
          "assets/img/AtenasBack2.jpg",
          "assets/img/AtenasBack3.jpg"
        ], {
            fade: 750,
            duration: 4000
        });
    </script>

</body>
</html>

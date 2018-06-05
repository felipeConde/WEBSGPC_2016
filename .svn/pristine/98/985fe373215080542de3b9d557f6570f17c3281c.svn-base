<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="altera_senha.aspx.vb" Inherits="altera_senha" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
   
    <div class="card" style="margin-top: 60px;">
                        <div class="card-header">
                            <h2>Alterar Senha <small>Digite sua nova senha para acessar o sistema.</small></h2>
                        </div>
                    
                        <div class="card-body card-padding">
                            <p class="c-black f-500 m-b-20">Senha</p>
                            
                            <div class="form-group">
                                <div class="fg-line">
                                    <asp:TextBox ID="txtsenha" runat="server" cssclass="form-control" rows="5" placeholder="Digite sua senha..."></asp:TextBox>
                                    
                                </div>
                            </div>
                            
                            <p class="c-black f-500 m-t-20 m-b-20">Confirme sua senha</p>
                            
                            <div class="form-group">
                                <div class="fg-line">
                                    <asp:TextBox ID="txtConfirmaSenha" runat="server" cssclass="form-control" rows="5" placeholder="Confirme sua senha..."></asp:TextBox>
                                </div>
                                
                            </div>
                           
                                <div class="form-group">
                                 
                                        <asp:Label class="alert alert-danger" role="alert" runat="server" id="divErro" Visible="false"></asp:Label>
                                 
                                    <asp:LinkButton ID="btGravar" runat="server" CssClass="btn bgm-blue waves-effect" Text="Gravar"></asp:LinkButton>
                                 
                                </div>
                           
                       
                    </div>
                   
       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FooterPlaceHolder1" Runat="Server">

    <script>

        function Sucesso2(msg) {
            swal({
                title: "Operação realizada com sucesso!",
                text: "Senha alterada",
                type: "success",
                showCancelButton: false,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "OK",
                cancelButtonText: "Não",
                closeOnConfirm: false
            }, function () {
                //swal("Deleted!", "Your imaginary file has been deleted.", "success");
                window.location.href = "default.aspx";
            });
            //swal("Here's a message!");

        }
    </script>
</asp:Content>




<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="RamalDetalhe.aspx.vb" Inherits="RamalDetalhe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<style>

    .table td{
        border: 1px solid #CCC;
        vertical-align: middle !important;
        padding-left: 0px !important;
        padding-right: 0px !important;
        text-align:center;
    }
    .table p {
        margin-bottom: 0px;
        text-align: center;
    }

</style>

      <div class="block-header">

            <h2>INFORMAÇÕES DO RAMAL</h2>
           
           </div>
     <div class="card">

        <div class="card-body card-padding">
            <div class="row">
                 <div class="col-sm-4">   
                     <h4>
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Ramal: </label><br />
                              <%=_registro.Numero_A %>                        
                          </div>
                         </h4>  
                 </div>

                 <div class="col-sm-4">   
                     <h4>
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Usuário: </label><br />
                              <%=_registro.Usuario %>                        
                          </div>
                         </h4>  
                 </div>

                 <div class="col-sm-4">   
                     <h4>
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">AR: </label><br />
                              <%=viewstate("ccusto") %>                  
                          </div>
                         </h4>  
                 </div>
                

              
            </div>

            </div>
            </div>

     <div class="card">

        <div class="card-body card-padding">
            <div class="row">
                 <div class="col-sm-4">   
                    
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Tarifável: </label><br />
                              <%=IIf(_registro.Tarifavel = "S", "SIM", "NÂO") %>                        
                          </div>
                         
                 </div>
                <div class="col-sm-4">   
                    
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Bloqueavel: </label><br />
                              <%=IIf(_registro.Bloqueavel = "S", "SIM", "NÂO") %>                        
                          </div>
                         
                 </div>
                <div class="col-sm-4">   
                    
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Ativo: </label><br />
                              <%=IIf(_registro.Ativo = "S", "SIM", "NÂO") %>                        
                          </div>
                         
                 </div>

                
            </div>

              <div class="row">

                

                 <div class="col-sm-4">   
                     
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Modelo: </label><br />
                              <%=ViewState("modelo") %>                  
                          </div>
                          
                 </div>

              
                 <div class="col-sm-4">   
                    
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Custo Ramal: </label><br />
                              <%=FormatCurrency(_registro.Custo_Ramal) %>                        
                          </div>
                          
                 </div>

                    <div class="col-sm-4">   
                    
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Custo Serviço: </label><br />
                              <%=FormatCurrency(_registro.CUSTO_SERVICO) %>                        
                          </div>
                          
                 </div>

               
            </div>

             

             

             <div class="row">
                 <div class="col-sm-12">   
                    
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">OBS: </label><br />
                              <%=_registro.OBS%>                        
                          </div>
                         
                 </div>
        </div>

            </div>       
         
            </div>

    <a href="GestaoRamais.aspx" class="btn btn-primary waves-effect">Voltar</a>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder1" Runat="Server">


</asp:Content>





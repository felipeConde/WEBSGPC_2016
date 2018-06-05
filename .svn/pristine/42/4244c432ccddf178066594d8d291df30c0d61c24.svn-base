<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="AparelhosMoveisDetalhes.aspx.vb" Inherits="AparelhosMoveisDetalhes" %>

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

            <h2>INFORMAÇÕES DO APARELHO</h2>
           
           </div>
     <div class="card">

        <div class="card-body card-padding">
            <div class="row">
                 <div class="col-sm-4">   
                     <h4>
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Linha: </label><br />
                              <%=_registro.TELEFONE %>                        
                          </div>
                         </h4>  
                 </div>

                 <div class="col-sm-4">   
                     <h4>
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Usuário: </label><br />
                              <%=_usuario.Nome_Usuario %>                        
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
                        <label class="fg-label">IMEI: </label><br />
                              <%=_registro.Identificacao %>                        
                          </div>
                         
                 </div>

                 <div class="col-sm-4">   
                    
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Marca: </label><br />
                              <%=ViewState("marca") %>                        
                          </div>
                          
                 </div>

                 <div class="col-sm-4">   
                     
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Modelo: </label><br />
                              <%=ViewState("modelo") %>                  
                          </div>
                          
                 </div>
            </div>

              <div class="row">
                 <div class="col-sm-4">   
                    
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Fornecedor: </label><br />
                              <%=ViewState("fornecedor")%>                        
                          </div>
                         
                 </div>

                 <div class="col-sm-4">   
                    
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Valor Aparelho: </label><br />
                              <%= formatCurrency(_registro.Valor_aparelho) %>                        
                          </div>
                          
                 </div>

                 <div class="col-sm-4">
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Status: </label><br />
                              <%=ViewState("status") %>                  
                          </div>
                          
                 </div>
            </div>

             <div class="row">
                 <div class="col-sm-4">   
                    
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Plano: </label><br />
                              <%=ViewState("plano")%>                        
                          </div>
                         
                 </div>

                 <div class="col-sm-4">                       
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">SIM Card: </label><br />
                              <%= _registro.simcard %>                        
                          </div>                          
                 </div>

                 <div class="col-sm-4">
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Valor SIMCARD: </label><br />
                              <%= formatCurrency(_registro.Simcard_value) %>                  
                       </div>
                          
                 </div>
            </div>

             <div class="row">
                 <div class="col-sm-4">   
                    
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Classificação: </label><br />
                              <%=ViewState("CLASSIFICACAO")%>                        
                          </div>
                         
                 </div>

                 <div class="col-sm-4">                       
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Nota Fiscal: </label><br />
                              <%= _registro.Nota_fiscal %>                        
                          </div>                          
                 </div>

                 <div class="col-sm-4">
                        <div class="form-group fg-line" >                                                
                        <label class="fg-label">Serial Number: </label><br />
                              <%= _registro.Serial_Number %>                  
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

    <a href="GestaoAparelhosMoveis.aspx" class="btn btn-primary waves-effect">Voltar</a>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder1" Runat="Server">


</asp:Content>




<%@ Application Language="VB" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.Http" %>
<%@ Import Namespace="System.Web.MVC" %>

<script runat="server">

    Sub Application_Start(sender As Object, e As EventArgs)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        RouteConfig.Register(GlobalConfiguration.Configuration)
        BundleConfig.RegisterBundles(BundleTable.Bundles)

        Dim config As HttpConfiguration = GlobalConfiguration.Configuration
        config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented
    End Sub

</script>

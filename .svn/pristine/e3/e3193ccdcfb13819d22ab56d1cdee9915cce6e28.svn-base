Imports System.Collections.Generic
Imports System.Web
Imports System.Web.Http
Imports System.Web.Routing
Imports Microsoft.AspNet.FriendlyUrls

Public Module RouteConfig
    Public Sub RegisterRoutes(routes As RouteCollection)
        Dim settings = New FriendlyUrlSettings()
        settings.AutoRedirectMode = RedirectMode.Permanent
        routes.EnableFriendlyUrls(settings)

    End Sub
    Public Sub Register(config As HttpConfiguration)

        config.Routes.MapHttpRoute(
          name:="DefaultApi",
          routeTemplate:="api/{controller}/{action}/{id}",
          defaults:=New With {.action = "Index", .id = RouteParameter.Optional}
      )

    End Sub

End Module

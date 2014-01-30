<%@ Page Language="C#" %>
<%@ Import Namespace="MiniMe" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <title>Simple Page</title>
    <%= Reference.StyleSheet
            .ReRenders.When.AnySourceFileChanges()
            .ForceRender()
            .Add("/Content/Site.css")
            .Add("/Content/Site2.css")
            .Render("/Content/Versions/#/Output.css") %>
</head>
<body>
    <form runat="server">
    <div class="page">
        <header>
            <h1>Hello from Simple Page</h1>
        </header>
        <section id="main">
            Some content here...
        </section>
        <footer>
        </footer>
    </div>
    </form>

    <%= Reference.Cdn
            .Add("https://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js", "/Scripts/jquery-1.5.1.js")
            .Render() %> 

    <%= Reference.JavaScript
            .ReRenders.Never()
            .ForceRender()
            .Add("/Scripts/jquery-1.5.1.js")
            .Add("/Scripts/modernizr-1.7.min.js")
            .Render("/Scripts/versions/#/Output.js") %> 
</body>
</html>

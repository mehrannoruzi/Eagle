namespace Eagle.CodeGenerator.Templates
{
    public static class ViewTemplate
    {
        public static string Manage =
@"
@model PagingListDetails<##entity##>
@{
    ViewBag.Title = Strings.Management+"" ""+DomainStrings.##entity##;
    Layout = ""_LayoutManage"";
}
@section styles
{
    <link href=""@Url.Content(""~/Styles/App/##entity##/manage.css"")"" rel=""stylesheet"" />
}

@section filter
{
    <partial name=""Partials/_Filters"" model=""new ##entity##SearchFilter()"" />
}
<partial name = ""Partials/_List"" model=""@Model"" />
@section scripts
{
    <script src=""@Url.Content(""~/Scripts/App/##entity##/manage.js"")""></script>
}
";
        public static string List = @"
@model PagingListDetails<##entity##>
@{
    Layout = null;
}
@if (Model.TotalCount > 0)
{
    <table class=""footable table table-hover toggle-arrow-tiny"">
        <thead>
            <tr>
                <th data-toggle=""true"">#</th>
##head##
                <th class=""text-center""><i class=""zmdi zmdi-wrench rotate-90""></i></th>
            </tr>
        </thead>
        <tbody>
@foreach(var item in Model.Items)
        {
                <tr>
                    <td>@(((Model.PageNumber - 1) * Model.PageSize) + Model.Items.IndexOf(item) + 1) </td>
##body##
                    <td class=""actions"">
                        <div class=""dropdown b-a-c"">
                            <span data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">
                                <i class=""zmdi zmdi-more rotate-90""></i>
                            </span>
                            <div class=""dropdown-menu"">
                                <a class=""dropdown-item update"" data-url=""@Url.Action(""Update"",""##entity##"",new { id = item.##entity##Id })"">
                                    <i class=""zmdi zmdi-edit default-i""></i>
                                    @Strings.Edit
                                </a>
                            </div>
                        </div>
                    </td>
                </tr>
            }
        </tbody>
        <tfoot class=""d-none"">
            <tr>
                <td colspan=""5"">
                    <ul class=""pagination float-right""></ul>
                </td>
            </tr>
        </tfoot>
    </table>
    <partial name = ""Partials/_Pagination"" model=""@Model"" />
}
else
{
    <div class=""alert alert-warning text-center"">
        <small>@string.Format(Strings.ThereIsNoRecord, Strings.Item)</small>
    </div>
}
";

        public static string Entity = @"
@model ##entity##
@{
    Layout = null;
}
@Html.HiddenFor(x => x.##entity##Id)
<div class=""row"">
##fields##
</div>
";
        public static string Filters = @"
@model ##entity##SearchFilter
@{
    Layout = null;
}

<div class=""row"">
##filters##
</div>
";
    }
}

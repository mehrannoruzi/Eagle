namespace Eagle.CodeGenerator
{
    public enum TemplateType : byte
    {
        Controller = 0,
        ManageView = 1,
        ListPartialView =2,
        FilterPartialView =3,
        EntityPartialView =4,
        IRepo = 5,
        Repo = 6,
        Srv = 7,
        ISrv = 8
    }
}

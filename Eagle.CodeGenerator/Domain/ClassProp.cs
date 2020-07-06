namespace Eagle.CodeGenerator
{
    public class ClassProp
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsEnum { get; set; }
        public bool IsMultilineText { get; set; }
        public bool IsForeignKey { get; set; }
        public bool IsPrimaryKey { get; set; }
    }
}

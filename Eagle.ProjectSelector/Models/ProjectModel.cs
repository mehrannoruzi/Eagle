namespace Eagle.ProjectSelector
{
    public class ProjectModel
    {
        public int Index { get; set; }
        public string SolutionFolder { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool Selected { get; set; }
    }
}

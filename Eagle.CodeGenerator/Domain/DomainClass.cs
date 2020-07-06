using System.Collections.Generic;

namespace Eagle.CodeGenerator
{
    public class DomainClass
    {
        public string Schema { get; set; }
        public string Name { get; set; }

        public List<ClassProp> Props { get; set; }
    }
}

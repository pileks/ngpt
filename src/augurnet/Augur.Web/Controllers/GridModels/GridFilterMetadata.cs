namespace Augur.Web.Controllers.GridModels
{
    public class GridFilterMetadata
    {
        public string Title { get; set; }
        public string Property { get; set; }
        public string Type { get; set; }
        public bool IsAugurEntity { get; set; }
        public bool IsEnum { get; set; }
        public GridFilterOperatorModel[] Operators { get; set; }
    }
}
using Atata;

namespace SomeApp2.UITests
{
    using _ = WorkspacePage;

    public class WorkspacePage : Page<_>
    {
        public int? WorkspaceId { get; set; }

        public static _ ById(int id)
        {
            return new _ { WorkspaceId = id };
        }

        protected override void Navigate()
        {
            Go.ToUrl($"/path/{WorkspaceId}/subpath");

            Go.To(new WorkspacePage { WorkspaceId = 5 });
            Go.To(WorkspacePage.ById(5));
        }
    }
}

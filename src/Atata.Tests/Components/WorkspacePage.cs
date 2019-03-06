using Atata;

namespace SomeApp.UITests
{
    using _ = WorkspacePage;

    public class WorkspacePage : Page<_>
    {
        public int? WorkspaceId { get; set; }

        protected override void Navigate()
        {
            Go.ToUrl($"/path/{WorkspaceId}/subpath");

            Go.To(new WorkspacePage { WorkspaceId = 5 });
        }
    }
}

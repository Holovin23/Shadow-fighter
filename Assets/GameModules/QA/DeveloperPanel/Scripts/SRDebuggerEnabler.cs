using SRDebugger.Services;
using SRF.Service;

namespace TFPlay.DeveloperUtilities
{
    public class SRDebuggerEnabler : BaseDeveloperTool
    {
        protected override void InitInternal()
        {

        }

        protected override void TogglePanel()
        {
            isOpened = !isOpened;
            SRServiceManager.GetService<IDebugService>().ShowDebugPanel(false);
        }
    }
}

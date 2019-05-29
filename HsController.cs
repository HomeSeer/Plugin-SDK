namespace HomeSeer.PluginSdk {

    public class HsController {

        private IHsController _interface;

        public HsController(IHsController hsController) {
            _interface = hsController;
        }

    }

}
namespace HomeSeer.PluginSdk {

    internal class HsController {

        private IHsController _interface;

        internal HsController(IHsController hsController) {
            _interface = hsController;
        }

    }

}
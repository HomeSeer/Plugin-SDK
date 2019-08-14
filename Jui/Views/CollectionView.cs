namespace HomeSeer.Jui.Views {

    internal class CollectionView : AbstractView {
        
        

        public CollectionView(string id) : base(id) { }
        public CollectionView(string id, string name) : base(id, name) { }
        
        public override string GetStringValue() {
            throw new System.NotImplementedException();
        }

        public override string ToHtml(int indent = 0) {
            throw new System.NotImplementedException();
        }

    }

}
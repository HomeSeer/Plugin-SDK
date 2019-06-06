namespace HomeSeer.Jui.Views {

    public class CollectionView : AbstractView {
        
        

        public CollectionView(string id) : base(id) { }
        public CollectionView(string id, string name) : base(id, name) { }
        
        public override string GetStringValue() {
            throw new System.NotImplementedException();
        }

        internal override string ToHtml(int indent = 0) {
            throw new System.NotImplementedException();
        }

    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views {

    public class SettingsCollection : IEnumerable<Page> {

	    [JsonIgnore]
	    public int Count => _pages?.Count ?? 0;

        [JsonProperty("pages")]
        public List<Page> Pages {
            get {
                if (_pageOrder == null || _pageOrder.Count == 0) {
                    return new List<Page>();
                }
                var pages = new List<Page>();
                foreach (var pageId in _pageOrder) {
                    if (!_pages.ContainsKey(pageId)) {
                        continue;
                    }
                    
                    pages.Add(_pages[pageId]);
                }

                return pages;
            }
            set {
                
                _pages = new Dictionary<string, Page>();
                _pageOrder = new List<string>();

                if (value == null || value.Count == 0) {
                    return;
                }

                foreach (var page in value) {
                    _pages.Add(page.Id, page);
                    _pageOrder.Add(page.Id);
                }
            }
        }

        [JsonProperty("selected_page")]
        public int SelectedPageIndex { get; set; } = 0;

        [JsonIgnore]
        private List<string> _pageOrder = new List<string>();
        
        [JsonIgnore]
        private Dictionary<string, Page> _pages = new Dictionary<string, Page>();

        public SettingsCollection() { }

        [JsonConstructor]
        public SettingsCollection(List<Page> pages, int selectedPageIndex) {
            Pages = pages;
            SelectedPageIndex = selectedPageIndex;
        }

        public void Add(Page page) {

            if (page?.Id == null) {
                throw new ArgumentNullException(nameof(page));
            }

            if (_pages.ContainsKey(page.Id)) {
                throw new ArgumentException("A page with that ID already exists in the collection", nameof(page));
            }
            
            _pages.Add(page.Id, page);
            _pageOrder.Add(page.Id);
        }
        
        public Page this[string pageId] {

            get {
                if (!_pages.ContainsKey(pageId)) {
                    //TODO
                }

                return _pages[pageId];
            }

            set {
                if (!_pages.ContainsKey(pageId)) {
                    //TODO
                }

                _pages[pageId] = value;
            }
        }

        public int IndexOf(string pageId) {

            if (string.IsNullOrWhiteSpace(pageId)) {
                throw new ArgumentNullException(nameof(pageId));
            }

            if (!_pages.ContainsKey(pageId)) {
                throw new KeyNotFoundException("A page with that ID was not found in the collection");
            }

            return _pageOrder.IndexOf(pageId);
        }

        public IEnumerator<Page> GetEnumerator() {
	        return Pages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
	        return GetEnumerator();
        }

        public void Swap(int index1, int index2) {

            if (index1 < 0 || index1 > _pages.Count) {
                throw new ArgumentOutOfRangeException(nameof(index1), "Index must be greater than -1 and less than or equal to the total number of pages in the collection");
            }
            
            if (index2 < 0 || index2 > _pages.Count) {
                throw new ArgumentOutOfRangeException(nameof(index2), "Index must be greater than -1 and less than or equal to the total number of pages in the collection");
            }

            var page1Id = _pageOrder[index1];
            var page2Id = _pageOrder[index2];
            _pageOrder[index1] = page2Id;
            _pageOrder[index2] = page1Id;
        }
        
        public void RemoveById(string pageId) {
            
            if (string.IsNullOrWhiteSpace(pageId)) {
                throw new ArgumentNullException(nameof(pageId));
            }

            if (!_pages.ContainsKey(pageId)) {
                throw new KeyNotFoundException("A page with that ID was not found in the collection");
            }

            _pages.Remove(pageId);
            _pageOrder.Remove(pageId);
        }

        public void RemoveAll() {
            
            _pages = new Dictionary<string, Page>();
            _pageOrder = new List<string>();
        }

        /// <summary>
        /// Serialize the settings collection as JSON
        /// </summary>
        /// <returns>A string containing the settings collection formatted as JSON</returns>
        public string ToJsonString() {
            return JsonConvert.SerializeObject(this, 
                                               Formatting.None, 
                                               new JsonSerializerSettings {
                                                                              TypeNameHandling = TypeNameHandling.Auto
                                                                          });
        }
        
        /// <summary>
        /// Deserialize a JSON string to a settings collection
        /// <para>
        /// This should always be wrapped in a try/catch in case the data received is malformed
        /// </para>
        /// </summary>
        /// <param name="jsonString">The JSON string containing the settings collection</param>
        /// <returns>A SettingsCollection</returns>
        /// <exception cref="JsonDataException">Thrown when there was a problem deserializing the settings collection</exception>
        public static SettingsCollection FromJsonString(string jsonString) {
	        try {
		        var collection = JsonConvert.DeserializeObject<SettingsCollection>(jsonString,
		                                                       new JsonSerializerSettings
		                                                       { TypeNameHandling = TypeNameHandling.Auto }
		                                                      );
				
		        return collection;
	        }
	        catch (JsonSerializationException exception) {
					
		        throw new JsonDataException(exception);
	        }
        }
        
        /// <summary>
		/// Convert the settings collection into a tabbed HTML page.
		/// </summary>
		/// <returns>A string containing a tabbed HTML page</returns>
		/// <exception cref="ArgumentNullException">Thrown if the list of pages is null</exception>
		/// <exception cref="ArgumentException">Thrown if the list of pages is empty</exception>
		public string ToHtml() {

			if (_pages == null) {
				throw new ArgumentNullException(nameof(_pages));
			}

			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (_pages.Count) {
				case 0:
					throw new ArgumentException("There are no pages to convert to HTML");
				case 1: {
					var page = _pages[_pageOrder[0]];
					return page.ToHtml();
				}
			}
			
			//TODO add the horizontal scrolling tab functionality
			var sb     = new StringBuilder();
			var indent = 0;
			//Open the tablist
			sb.Append("<ul class=\"nav nav-tabs hs-tabs\" role=\"tablist\">");
			sb.Append(Environment.NewLine);
			indent++;
			//Add tabs
			var pageIndex = -1;
			foreach (var pageId in _pageOrder) {
				var page = _pages[pageId];
				pageIndex++;
				sb.Append(AbstractView.GetIndentStringFromNumber(indent));
				//Open nav item
				sb.Append("<li class=\"nav-item\">");
				sb.Append(Environment.NewLine);
				indent++;
				sb.Append(AbstractView.GetIndentStringFromNumber(indent));
				//Open anchor
				sb.Append("<a class=\"nav-link waves-light\" id=\"");
				sb.Append(page.Id);
				sb.Append(".tab\" data-toggle=\"tab\" href=\"#");
				sb.Append(page.Id);
				sb.Append("\" role=\"tab\" aria-controls=\"");
				sb.Append(page.Id);
				sb.Append("\" aria-selected=\"");
				sb.Append(pageIndex == SelectedPageIndex ? "true" : "false");
				sb.Append("\">");
				sb.Append(page.Name);
				//Close anchor
				sb.Append("</a>");
				sb.Append(Environment.NewLine);
				indent--;
				sb.Append(AbstractView.GetIndentStringFromNumber(indent));
				//Close nav item
				sb.Append("</li>");
				sb.Append(Environment.NewLine);
			}
			
			//Close the tablist
			sb.Append("</ul>");
			sb.Append(Environment.NewLine);
			//Open the content div
			sb.Append("<div class=\"tab-content container\">");
			sb.Append(Environment.NewLine);
			pageIndex = -1;
			//Add the tab content divs
			foreach (var pageId in _pageOrder) {
				var page = _pages[pageId];
				pageIndex++;
				sb.Append(AbstractView.GetIndentStringFromNumber(indent));
				//Open the tab pane div
				sb.Append("<div class=\"tab-pane fade");
				if (pageIndex == 0) {
					sb.Append(" active show");
				}
				sb.Append("\" role=\"tabpanel\" aria-labelledby=\"");
				sb.Append(page.Id);
				sb.Append(".tab\" id=\"");
				sb.Append(page.Id);
				sb.Append("\">");
				sb.Append(Environment.NewLine);
				indent++;
				//Add the page content
				sb.Append(page.ToHtml(indent));
				indent--;
				sb.Append(AbstractView.GetIndentStringFromNumber(indent));
				//Close the tab content div
				sb.Append("</div>");
				sb.Append(Environment.NewLine);
			}
			//Close the content div
			sb.Append("</div>");

			return sb.ToString();
		}

    }

}
using System;
using System.Collections.Generic;
using HomeSeer.Jui.Types;

namespace HomeSeer.Jui.Views {

    internal static class ViewCollectionHelper {

	    #region Create/Add
	    
	    /// <summary>
	    /// Add a view to a collection
	    /// </summary>
	    /// <param name="view">The view to add to the collection</param>
	    /// <param name="viewList">A reference to the current list of views</param>
	    /// <param name="viewIds">A reference to the map of view IDs and list indexes</param>
	    /// <param name="toGroup">If the collection the view is being added it is a ViewGroup. DEFAULT: false for a Page</param>
	    /// <exception cref="ArgumentNullException">The view or its ID is null</exception>
	    /// <exception cref="ArgumentException">There is already a view with the same ID present in the collection</exception>
	    /// <exception cref="InvalidOperationException">Thrown when trying to add a ViewGroup to another ViewGroup</exception>
        /// <exception cref="ViewTypeMismatchException">Thrown when a view group's type does not match its class</exception>
	    internal static void AddView(AbstractView view, 
                                   ref List<AbstractView> viewList, 
                                   ref Dictionary<string, int> viewIds,
                                   bool toGroup = false) {
            
            if (viewList == null) {
                viewList = new List<AbstractView>();
                viewIds = new Dictionary<string, int>();
            }

            if (view?.Id == null) {
                throw new ArgumentNullException(nameof(view), "The view or its ID is null");
            }

            if (viewIds.ContainsKey(view.Id)) {
                throw new ArgumentException("A view with that ID already exists in the collection");
            }
            
            var viewIndex = viewList.Count;

            if (view.Type == EViewType.Group) {
	            
	            if (toGroup) {
		            throw new InvalidOperationException("View groups cannot be nested");
	            }

	            if (!(view is ViewGroup viewGroup)) {
		            throw new ViewTypeMismatchException();
	            }

	            //Add all of the view group's view IDs to the map of view IDs pointing to the view group
	            // so any update or get calls are funneled through the group
	            foreach (var viewGroupView in viewGroup.Views) {
		            viewIds.Add(viewGroupView.Id, viewIndex);
	            }
            }
            
            viewList.Add(view);
            viewIds.Add(view.Id, viewIndex);
        }
        
	    /// <inheritdoc cref="AddView"/>
	    /// <summary>
	    /// Add multiple views to a collection
	    /// </summary>
	    /// <param name="views">The views to add to the collection</param>
	    /// <param name="viewList">A reference to the current list of views</param>
	    /// <param name="viewIds">A reference to the map of view IDs and list indexes</param>
	    /// <param name="toGroup">If the collection the views are being added it is a ViewGroup. DEFAULT: false for a Page</param>
	    /// <exception cref="ArgumentNullException">The list of views is null</exception>
        internal static void AddViews(IEnumerable<AbstractView>   views,
                                      ref List<AbstractView>      viewList, 
                                      ref Dictionary<string, int> viewIds,
                                      bool                        toGroup = false) {
			
	        if (views == null) {
		        throw new ArgumentNullException(nameof(views));
	        }
			
	        foreach (var view in views) {
				
		        AddView(view, ref viewList, ref viewIds, toGroup);
	        }
        }

	    /// <inheritdoc cref="AddViews"/>
	    /// <summary>
	    /// Set the collection of views
	    /// </summary>
	    /// <param name="views">The new state of the collection</param>
	    /// <param name="viewList">A reference to the current list of views</param>
	    /// <param name="viewIds">A reference to the map of view IDs and list indexes</param>
	    /// <param name="toGroup">If the collection the views are being added it is a ViewGroup. DEFAULT: false for a Page</param>
        internal static void SetViews(IEnumerable<AbstractView>   views,
                                      ref List<AbstractView>      viewList, 
                                      ref Dictionary<string, int> viewIds,
                                      bool                        toGroup = false) {

		    viewList = new List<AbstractView>();
		    viewIds  = new Dictionary<string, int>();
		    
	        AddViews(views, ref viewList, ref viewIds, toGroup);
        }
        
        #endregion
        
        #region Read
		
        /// <summary>
        /// Check to see if a view with a specific ID is present in a collection
        /// </summary>
        /// <param name="viewId">The ID of the view to look for</param>
        /// <param name="viewList">A reference to the current list of views</param>
        /// <param name="viewIds">A reference to the map of view IDs and list indexes</param>
        /// <returns>TRUE if the view exists in the collection; FALSE if it does not exist in the collection</returns>
        /// <exception cref="ArgumentNullException">The viewId to look for is NULL</exception>
        /// <exception cref="IndexOutOfRangeException">The ID was found, but the view is not in the collection</exception>
		internal static bool ContainsViewWithId(string viewId,
		                                        ref List<AbstractView>      viewList, 
		                                        ref Dictionary<string, int> viewIds) {
			
			if (string.IsNullOrWhiteSpace(viewId)) {
				throw new ArgumentNullException(nameof(viewId));
			}

			if (viewList == null || viewList.Count == 0) {
				return false;
			}

			try {
				var viewIndex = viewIds[viewId];
				if (viewIndex >= viewList.Count) {
					throw new IndexOutOfRangeException("That ID points to a view that does not exist in the collection.");
				}
				
				return true;
			}
			catch (KeyNotFoundException) {
				return false;
			}
		}

        /// <summary>
        /// Get the view with a specific ID from a collection
        /// </summary>
        /// <param name="viewId">The ID of the view to look for</param>
        /// <param name="viewList">A reference to the current list of views</param>
        /// <param name="viewIds">A reference to the map of view IDs and list indexes</param>
        /// <returns>The view with the specified ID</returns>
        /// <exception cref="ArgumentNullException">The viewId to look for is NULL</exception>
        /// <exception cref="IndexOutOfRangeException">The ID was found, but the view is not in the collection</exception>
        /// <exception cref="ArgumentException">There are no views in the collection</exception>
        /// <exception cref="ViewNotFoundException">No views with that ID were found in the collection</exception>
        /// <exception cref="ViewTypeMismatchException">Thrown when a view group's type does not match its class</exception>
		internal static AbstractView GetViewById(string viewId,
		                                         ref List<AbstractView>      viewList, 
		                                         ref Dictionary<string, int> viewIds) {
			
			if (string.IsNullOrWhiteSpace(viewId)) {
				throw new ArgumentNullException(nameof(viewId));
			}

			if (viewList == null || viewList.Count == 0) {
				throw new ArgumentException("There are no views in this collection");
			}

			try {
				var viewIndex = viewIds[viewId];
				if (viewIndex >= viewList.Count) {
					throw new IndexOutOfRangeException("That ID points to a view that does not exist in the collection.");
				}
				
				var foundView = viewList[viewIndex];
				if (foundView.Id == viewId) {
					return foundView;
				}
				
				if (foundView.Type == EViewType.Group) {
					
					if (!(foundView is ViewGroup viewGroup)) {
						throw new ViewTypeMismatchException();
					}

					return viewGroup.GetViewById(viewId);
				}
				
			}
			catch (KeyNotFoundException exception) {
				throw new ViewNotFoundException("There are no views with that ID in the collection", exception);
			}

			throw new ViewNotFoundException("There are no views with that ID in the collection");
        }
        
        /// <summary>
        /// Get the view with a specific ID from a collection cast as the target type
        /// </summary>
        /// <param name="viewId">The ID of the view to look for</param>
        /// <param name="viewList">A reference to the current list of views</param>
        /// <param name="viewIds">A reference to the map of view IDs and list indexes</param>
        /// <returns>The view with the specified ID cast to the target type</returns>
        /// <exception cref="ArgumentNullException">The viewId to look for is NULL</exception>
        /// <exception cref="IndexOutOfRangeException">The ID was found, but the view is not in the collection</exception>
        /// <exception cref="ArgumentException">There are no views in the collection</exception>
        /// <exception cref="ViewNotFoundException">No views with that ID were found in the collection</exception>
        /// <exception cref="ViewTypeMismatchException">Thrown when a view group's type does not match its class</exception>
		internal static TViewType GetViewById<TViewType>(string viewId,
		                                         ref List<AbstractView>      viewList, 
		                                         ref Dictionary<string, int> viewIds) where TViewType : AbstractView {
			
			if (string.IsNullOrWhiteSpace(viewId)) {
				throw new ArgumentNullException(nameof(viewId));
			}

			if (viewList == null || viewList.Count == 0) {
				throw new ArgumentException("There are no views in this collection");
			}

			try {
				var viewIndex = viewIds[viewId];
				if (viewIndex >= viewList.Count) {
					throw new IndexOutOfRangeException("That ID points to a view that does not exist in the collection.");
				}
				
				var foundView = viewList[viewIndex];
				if (foundView.Id == viewId) {
					if (!(foundView is TViewType foundViewCast)) {
						throw new ViewTypeMismatchException();
					}
					return foundViewCast;
				}
				
				if (foundView.Type == EViewType.Group) {
					
					if (!(foundView is ViewGroup viewGroup)) {
						throw new ViewTypeMismatchException();
					}

					if (!(viewGroup.GetViewById(viewId) is TViewType foundViewCast)) {
						throw new ViewTypeMismatchException();
					}
					
					return foundViewCast;
				}
				
			}
			catch (KeyNotFoundException exception) {
				throw new ViewNotFoundException("There are no views with that ID in the collection", exception);
			}

			throw new ViewNotFoundException("There are no views with that ID in the collection");
        }
		
		#endregion
		
		#region Update

		/// <summary>
		/// Perform a soft update to a view in a collection with a particular ID
		/// </summary>
		/// <param name="view">The new state of the view to update</param>
		/// <param name="viewList">A reference to the current list of views</param>
		/// <param name="viewIds">A reference to the map of view IDs and list indexes</param>
		/// <exception cref="ArgumentNullException">The viewId to look for is NULL</exception>
		/// <exception cref="IndexOutOfRangeException">The ID was found, but the view is not in the collection</exception>
		/// <exception cref="ArgumentException">There are no views in the collection</exception>
		/// <exception cref="ViewNotFoundException">No views with that ID were found in the collection</exception>
		internal static void UpdateViewById(AbstractView view,
		                                    ref List<AbstractView>      viewList, 
		                                    ref Dictionary<string, int> viewIds) {

			if (view == null) {
				throw new ArgumentNullException(nameof(view));
			}
			
			if (viewList == null || viewList.Count == 0) {
				throw new ArgumentException("There are no views in this collection");
			}
			
			try {
				var viewIndex = viewIds[view.Id];
				if (viewIndex >= viewList.Count) {
					throw new IndexOutOfRangeException("That ID points to a view that does not exist in the collection.");
				}
				
				var foundView = viewList[viewIndex];
				if (foundView.Id == view.Id) {
					foundView.Update(view);
					return;
				}
				
				if (foundView.Type == EViewType.Group) {
					
					if (!(foundView is ViewGroup viewGroup)) {
						throw new ViewTypeMismatchException();
					}

					viewGroup.UpdateViewById(view);
					return;
				}

			}
			catch (KeyNotFoundException exception) {
				throw new ViewNotFoundException("There are no views with that ID in the collection", exception);
			}

			throw new ViewNotFoundException("There are no views with that ID in the collection");
		}

		internal static void UpdateViewValueById(string id, 
		                                         string value,
		                                         ref List<AbstractView>      viewList, 
		                                         ref Dictionary<string, int> viewIds) {
			
			if (viewList == null || viewList.Count == 0) {
				throw new ArgumentException("There are no views in this collection");
			}
			
			try {
				var viewIndex = viewIds[id];
				if (viewIndex >= viewList.Count) {
					throw new IndexOutOfRangeException("That ID points to a view that does not exist in the collection.");
				}
				
				var foundView = viewList[viewIndex];
				if (foundView.Id == id) {
					foundView.UpdateValue(value);
					return;
				}
				
				if (foundView.Type == EViewType.Group) {
					
					if (!(foundView is ViewGroup viewGroup)) {
						throw new ViewTypeMismatchException();
					}

					viewGroup.UpdateViewValueById(id, value);
					return;
				}

			}
			catch (KeyNotFoundException exception) {
				throw new ViewNotFoundException("There are no views with that ID in the collection", exception);
			}

			throw new ViewNotFoundException("There are no views with that ID in the collection");
			
		}
		
		/// <summary>
		/// Build a map of view IDs and list indexes for a collection of views
		/// </summary>
		/// <param name="viewList">A reference to the current list of views</param>
		/// <param name="viewIds">A reference to the map of view IDs and list indexes</param>
		/// <param name="calledByGroup">Whether the method was called by a ViewGroup or not. Prevents infinite recursion.</param>
		/// <exception cref="ArgumentNullException">The list of views to map is null</exception>
		internal static void MapViewIds(List<AbstractView> viewList, out Dictionary<string, int> viewIds, bool calledByGroup = false) {

			if (viewList == null) {
				throw new ArgumentNullException(nameof(viewList));
			}
		    
			viewIds = new Dictionary<string, int>();

			//Loop through the list of views
			for (var i = 0; i < viewList.Count; i++) {
				var view = viewList[i];
				viewIds.Add(view.Id, i);
			    
				//Don't process any of sub-views if the caller is already a ViewGroup
				if (calledByGroup) {
					continue;
				}
			    
				if (!(view is ViewGroup viewGroup)) {
					continue;
				}
			    
				//Tell the view group to map its view IDs before adding them to this map
				viewGroup.MapViewIds();
				//Add the view group's IDs to this map
				foreach (var viewGroupViewId in viewGroup.ViewIds) {
					viewIds.Add(viewGroupViewId, i);
				}
			}
		}
		
		#endregion
		
		#region Delete

		/// <summary>
		/// Remove a view with the specified ID from the collection.
		/// <para>
		/// Remaining views at indexes greater than the specified view will be moved down one index to fill the empty space
		/// </para>
		/// </summary>
		/// <param name="viewId">The ID of the view to remove</param>
		/// <param name="viewList">The current list of views in the collection</param>
		/// <param name="viewIds">The current view ID to index map for the collection</param>
		/// <exception cref="ArgumentNullException">
		/// Thrown when any of the supplied parameters is null of empty
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Thrown when the number of items in the <paramref name="viewList"/> and <paramref name="viewIds"/> do not match
		/// </exception>
		/// <exception cref="KeyNotFoundException">Thrown when a view with the specified ID was not found</exception>
		internal static void RemoveViewById(string viewId, ref List<AbstractView> viewList,
		                                    ref Dictionary<string, int> viewIds) {
			if (string.IsNullOrWhiteSpace(viewId)) {
				throw new ArgumentNullException(nameof(viewId), "Invalid view ID specified.");
			}

			if (viewList == null || viewList.Count == 0) {
				throw new ArgumentNullException(nameof(viewList), "Cannot remove a view from an empty list.");
			}

			if (viewIds == null || viewIds.Count == 0) {
				throw new ArgumentNullException(nameof(viewIds), "Cannot remove a view from an empty list");
			}

			if (viewList.Count != viewIds.Count) {
				throw new ArgumentException("The number of views and IDs do not match");
			}

            var index = viewIds[viewId];
            if (!viewIds.Remove(viewId))
            {
                throw new KeyNotFoundException("No view with that ID exists in the collection");
            }

            // 09-26-2020 sjhill01: RemoveViewById does not remove views PSDK-125, GitHub PSDK #95 // 02-09-2021 JLW: Fix date and issue keys
            var numViews = viewList.Count;
            var newList = new List<AbstractView>();
            for (var i = 0; i < numViews; i++)
            {

                // if it's below the removed index, add it as-is
                if (i < index)
                {
                    newList.Add(viewList[i]);
                }
                // if it equals the removed index, do nothing
                // if it's above the removed index, move the rest of the IDs down one
                else if (i > index)
                {
                    newList.Add(viewList[i]);
                    viewIds[viewList[i].Id] = i - 1;
                }
            }

            viewList = new List<AbstractView>(newList);
        }

        /// <summary>
        /// Trim all views in the collection following the view with the specified ID
        /// </summary>
        /// <param name="viewId">The ID of the view that should be at the end of the collection</param>
        /// <param name="viewList">The current list of views in the collection</param>
        /// <param name="viewIds">The current view ID to index map for the collection</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any of the supplied parameters is null of empty
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the number of items in the <paramref name="viewList"/> and <paramref name="viewIds"/> do not match
        /// </exception>
        internal static void RemoveViewsAfterId(string viewId, ref List<AbstractView> viewList,
		                                        ref Dictionary<string, int> viewIds) {
			if (string.IsNullOrWhiteSpace(viewId)) {
				throw new ArgumentNullException(nameof(viewId), "Invalid view ID specified.");
			}

			if (viewList == null || viewList.Count == 0) {
				throw new ArgumentNullException(nameof(viewList), "Cannot remove a view from an empty list.");
			}

			if (viewIds == null || viewIds.Count == 0) {
				throw new ArgumentNullException(nameof(viewIds), "Cannot remove a view from an empty list");
			}

			if (viewList.Count != viewIds.Count) {
				throw new ArgumentException("The number of views and IDs do not match");
			}
			
			var lastIndex = viewIds[viewId];
			var newList  = new List<AbstractView>();
			var newDict  = new Dictionary<string, int>();
			for (var i = 0; i <= lastIndex; i++) {

				var curView = viewList[i];
				newList.Add(curView);
				newDict.Add(curView.Id, i);
			}

			viewList = new List<AbstractView>(newList);
			viewIds = new Dictionary<string, int>(newDict);
		}

		/// <summary>
		/// Clear a collection of views
		/// </summary>
		/// <param name="viewList">A reference to the current list of views</param>
		/// <param name="viewIds">A reference to the map of view IDs and list indexes</param>
		internal static void RemoveAllViews(out List<AbstractView>      viewList, 
		                                    out Dictionary<string, int> viewIds) {
			
			viewList = new List<AbstractView>();
			viewIds = new Dictionary<string, int>();
		}
		
		#endregion

    }

}
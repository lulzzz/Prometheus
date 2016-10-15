﻿using System;
using System.Collections.Generic;

namespace Prometheus.WebUI.Models.Shared
{
	/// <summary>
	/// For use with the _ItemListView Partial View
	/// Each item in the list will have an action link on the same controller to the action/id
	/// </summary>
	public class LinkListModel
	{
		public LinkListModel() { }

	    public LinkListModel(IEnumerable<KeyValuePair<Guid, string>> listItems, string selectAction, string title)
	    {
            SelectAction = selectAction;
            ListItems = listItems;
            Title = title;
        }

		public LinkListModel(IEnumerable<KeyValuePair<Guid, string>> listItems, Guid? selectedItemId, string selectAction, string addAction, string title):this(listItems, selectAction, title)
		{
			SelectedItemId = selectedItemId;
		    AddAction = addAction;
		}

		public IEnumerable<KeyValuePair<Guid, string>> ListItems { get; set; }
		public Guid? SelectedItemId { get; set; }
		public string SelectAction { get; set; }
        public string AddAction { get; set; }
	    public string Title { get; set; }
    }
}
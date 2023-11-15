﻿using System;
using System.Windows.Input;

namespace Kalendarzyk4s.ViewModels
{/// <summary>
 /// Button that can be selected and deselected with a border around it
 /// Also has a command that can be executed when the button is selected
 /// </summary>
	public class SelectableButtonViewModel : BaseViewModel
	{
		private bool _isSelected = false;
		private int _borderSize = 7;
		private const int FullOpacity = 1;
		private float FadedOpacity = 0.3f;

		public string ButtonText { get; set; }
		public Color ButtonColor { get; set; }
		public ICommand ButtonCommand { get; set; }

		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				if (_isSelected == value) { return; }
				_isSelected = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(ButtonBorder));
				OnPropertyChanged(nameof(ButtonOpacity));
			}
		}
		public float ButtonOpacity => IsSelected ? FullOpacity : FadedOpacity;
		public int ButtonBorder => IsSelected ? 0 : _borderSize;
		public SelectableButtonViewModel() { }

		public SelectableButtonViewModel(string text = null, bool isSelected = false, ICommand selectButtonCommand = null, int borderSize = 7, float fadedOpacity = 0.3f)
		{
			IsSelected = isSelected;
			ButtonText = text;
			ButtonCommand = selectButtonCommand;
			_borderSize = borderSize;
			FadedOpacity = fadedOpacity;
		}
	}
}

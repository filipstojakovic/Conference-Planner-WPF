using System.ComponentModel;
using System.Runtime.CompilerServices;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.dialog;

public class EventDialogModel : INotifyPropertyChanged
{
	private Event _event;
	private string _url = "";
	private string _city = "";
	private string _address = "";
	private bool _isLive = true;
	private bool _isReadOnly = false;

	public Event EventDialog
	{
		get { return _event; }
		set
		{
			_event = value;
			OnPropertyChanged();
		}
	}

	public string Url
	{
		get { return _url; }
		set
		{
			_url = value;
			OnPropertyChanged();
		}
	}

	public string City
	{
		get { return _city; }
		set
		{
			_city = value;
			OnPropertyChanged();
		}
	}

	public string Address
	{
		get { return _address; }
		set
		{
			_address = value;
			OnPropertyChanged();
		}
	}

	public bool IsLive
	{
		get { return _isLive; }
		set
		{
			_isLive = value;
			OnPropertyChanged();
		}
	}

	public bool IsReadOnly
	{
		get { return _isReadOnly; }
		set
		{
			_isReadOnly = value;
			OnPropertyChanged();
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	public void OnPropertyChanged([CallerMemberName] string name = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
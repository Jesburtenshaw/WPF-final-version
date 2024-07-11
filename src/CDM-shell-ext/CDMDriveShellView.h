#pragma once

#include "resource.h"       // main symbols
#include "cdmshellext_i.h"
#include <memory>
#include "clrloadersimple.h"
#pragma comment(lib, "mscoree.lib")

class Singleton {
public:
	// Delete copy constructor and assignment operator to prevent copies
	Singleton(const Singleton&) = delete;
	Singleton& operator=(const Singleton&) = delete;

	// Static method to get the single instance of the class
	static Singleton& getInstance() {
		static Singleton instance; // Guaranteed to be destroyed and instantiated on first use
		return instance;
	}

	// Method to get the value
	int getValue() const {
		return value;
	}

	// Method to set the value
	void setValue(int newValue) {
		value = newValue;
	}

	// Method to get the value
	IDispatchPtr getPtrValue() const {
		return mcdmPtr;
	}

	// Method to set the value
	void setPtrValue(IDispatchPtr newValue) {
		mcdmPtr = newValue;
	}

private:
	int value; // The integer value to be stored
	IDispatchPtr mcdmPtr;
	// Private constructor to prevent instantiation
	Singleton() : value(0) {}
};



class ATL_NO_VTABLE CDMDriveShellView :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CDMDriveShellView, &CLSID_CDMDriveShellView>,
	public IShellView,
	public IOleCommandTarget,
	public CWindowImpl<CDMDriveShellView>
{
public:
	CDMDriveShellView();
	~CDMDriveShellView();

	DECLARE_NO_REGISTRY()
	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct();
	void FinalRelease();

	DECLARE_WND_CLASS(NULL)

	BEGIN_COM_MAP(CDMDriveShellView)
		COM_INTERFACE_ENTRY(IShellView)
		COM_INTERFACE_ENTRY(IOleWindow)
		COM_INTERFACE_ENTRY(IOleCommandTarget)
	END_COM_MAP()

	BEGIN_MSG_MAP(CShellViewImpl)
		MESSAGE_HANDLER(WM_CREATE, OnCreate)
		MESSAGE_HANDLER(WM_SIZE, OnSize)
		MESSAGE_HANDLER(WM_KEYDOWN, OnKeyDown)
		MESSAGE_HANDLER(WM_SETFOCUS, OnSetFocus)
		MESSAGE_HANDLER(WM_CONTEXTMENU, OnContextMenu)
	END_MSG_MAP()

	// IOleWindow
	IFACEMETHODIMP GetWindow(_Out_ HWND* phwnd);
	IFACEMETHODIMP ContextSensitiveHelp(_In_ BOOL fEnterMode);

	// IShellView methods
	IFACEMETHODIMP CreateViewWindow(_In_ LPSHELLVIEW pPrevious, _In_ LPCFOLDERSETTINGS pfs, _In_ LPSHELLBROWSER psb, _In_ LPRECT prcView, _Out_ HWND* phWnd);
	IFACEMETHODIMP DestroyViewWindow();
	IFACEMETHODIMP GetCurrentInfo(_Out_ LPFOLDERSETTINGS pfs);
	IFACEMETHODIMP Refresh();
	IFACEMETHODIMP UIActivate(_In_ UINT uState);
	IFACEMETHODIMP AddPropertySheetPages(_In_ DWORD dwReserved, _In_ LPFNSVADDPROPSHEETPAGE pfn, _In_ LPARAM lparam);
	IFACEMETHODIMP EnableModeless(_In_ BOOL fEnable);
	IFACEMETHODIMP GetItemObject(_In_ UINT uItem, _In_ REFIID riid, _Out_ LPVOID* ppv);
	IFACEMETHODIMP SaveViewState();
	IFACEMETHODIMP SelectItem(_In_ PCUITEMID_CHILD pidlItem, _In_ SVSIF uFlags);
	IFACEMETHODIMP TranslateAccelerator(LPMSG msg);

	// IOleCommandTarget methods
	IFACEMETHODIMP QueryStatus(_In_ const GUID* pguidCmdGroup, _In_ ULONG cCmds, _In_ OLECMD prgCmds[], _Out_ OLECMDTEXT* pCmdText);
	IFACEMETHODIMP Exec(_In_ const GUID* pguidCmdGroup, _In_ DWORD nCmdID, _In_ DWORD nCmdexecopt, _In_ VARIANT* pvaIn, _Out_ VARIANT* pvaOut);

	// Message handlers
	LRESULT OnCreate(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnSize(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnSetFocus(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnContextMenu(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnKeyDown(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);

	// Implementation
	// Other stuff
	void Init(CComPtr<IShellFolder>& pContainingFolder);

	static UINT sm_uListID;
private:
	HWND					m_hwndParent; // parent window
	HMENU					m_hMenu; // current shell window
	CComPtr<IShellFolder>	m_pContainingFolder; // holds the ref to container folder
	CComPtr<IShellBrowser>	m_pShellBrowser; // ref to the ShellBrowser pointer
	FOLDERSETTINGS			m_FolderSettings;
	UINT					m_uUIState;

	CWindow							m_wndHost;
	std::unique_ptr<CCLRLoaderSimple> m_pCLRLoader;
	IDispatchPtr m_cdmPtr;

	void _HandleActivate(UINT uState);
	void _HandleDeactivate();
	void _FillList();
	void LoadCDM(HWND hWnd, HWND hWndParent, HWND hWndLeft);

	HRESULT CallMethod(LPCWSTR assemblyName, LPCWSTR className, LPCWSTR methodName, LONGLONG param);

};


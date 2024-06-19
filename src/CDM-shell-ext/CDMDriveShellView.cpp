#include "pch.h"
#include "CDMDriveShellView.h"

UINT CDMDriveShellView::sm_uListID = 101;


BOOL CALLBACK EnumChildProc(HWND hwnd, LPARAM lParam) {
	char className[256];
	GetClassNameA(hwnd, className, sizeof(className));

	if (std::string(className) == "SysTreeView32") {
		std::cout << "Found left side panel HWND (SysTreeView32): " << hwnd << std::endl;
		*((HWND*)lParam) = hwnd;
		return FALSE; // Stop enumeration once we found the target window
	}

	return TRUE; // Continue enumeration
}

HWND GetFileExplorerLeftPanelHWND() {
	// Find the File Explorer window
	HWND explorerHwnd = FindWindowA("CabinetWClass", NULL);
	if (!explorerHwnd) {
		std::cerr << "File Explorer window not found." << std::endl;
		return NULL;
	}

	// Enumerate child windows
	HWND leftPanelHwnd = NULL;
	EnumChildWindows(explorerHwnd, EnumChildProc, (LPARAM)&leftPanelHwnd);

	return leftPanelHwnd;
}


CDMDriveShellView::CDMDriveShellView()
	: m_uUIState(SVUIA_DEACTIVATE),
	m_hwndParent(nullptr),
	m_hMenu(nullptr),
	m_pContainingFolder(nullptr)
{
	LOGINFO(_MainApplication->GetLogger(), L"CDMDriveShellView constructor called");
}

CDMDriveShellView::~CDMDriveShellView()
{
	LOGINFO(_MainApplication->GetLogger(), L"CDMDriveShellView destructor called");
}

HRESULT CDMDriveShellView::FinalConstruct()
{
	LOGINFO(_MainApplication->GetLogger(), L"FinalConstruct called");
	return S_OK;
}

void CDMDriveShellView::FinalRelease()
{
	LOGINFO(_MainApplication->GetLogger(), L"FinalRelease called. Release the container folder");
	m_pContainingFolder.Release();
	m_pContainingFolder = nullptr;
}


IFACEMETHODIMP CDMDriveShellView::GetWindow(_Out_ HWND* phwnd)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"Return the container window's handle to the browser");
		*phwnd = m_hWnd;
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CDMDriveShellView::ContextSensitiveHelp(_In_ BOOL fEnterMode)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"ContextSensitiveHelp: fEnterMode = %d", fEnterMode);
		LOGINFO(_MainApplication->GetLogger(), L"Doesn't support еру рудз during an in-place activation session");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

//HRESULT CallIDispatchMethod(IDispatch* pDisp, LPCWSTR name, CComVariant params[], int numParams, CComVariant& result)
//{
//	if (pDisp == NULL)
//		return E_POINTER;
//	DISPID dispId;
//	LPOLESTR v1[] = { (LPOLESTR)name };
//	HRESULT hr = pDisp->GetIDsOfNames(IID_NULL, v1, 1, LOCALE_SYSTEM_DEFAULT, &dispId);
//	if (FAILED(hr))
//	{
//		return hr;
//	}
//	DISPPARAMS dp;
//	dp.cNamedArgs = 0;
//	dp.cArgs = numParams;
//	dp.rgdispidNamedArgs = NULL;
//	dp.rgvarg = params;
//	EXCEPINFO exInfo;
//	UINT n = 0;
//	hr = pDisp->Invoke(dispId, IID_NULL, LOCALE_SYSTEM_DEFAULT, DISPATCH_METHOD, &dp, &result, &exInfo, &n);
//	if (FAILED(hr))
//	{
//		return hr;
//	}
//	return S_OK;
//}

HRESULT CallIDispatchMethod(IDispatch* pDisp, LPCWSTR name, CComVariant param1, CComVariant param2, CComVariant param3, CComVariant& result)
{
	if (pDisp == NULL)
	{
		return E_POINTER;
	}

	// Get the DISPID for the method name
	DISPID dispId;
	LPOLESTR v1[] = { (LPOLESTR)name };
	HRESULT hr = pDisp->GetIDsOfNames(IID_NULL, v1, 1, LOCALE_SYSTEM_DEFAULT, &dispId);
	if (FAILED(hr))
	{
		return hr;
	}

	// Prepare the DISPPARAMS structure
	DISPPARAMS dp;
	dp.cNamedArgs = 0;
	dp.cArgs = 3; // Number of parameters
	dp.rgdispidNamedArgs = NULL;

	// Create an array of variants for the parameters
	CComVariant params[3] = { param3, param2, param1 }; // Note the order: last parameter first

	dp.rgvarg = params;

	// Initialize the EXCEPINFO structure
	EXCEPINFO exInfo = {};
	UINT n = 0;

	// Invoke the method
	hr = pDisp->Invoke(dispId, IID_NULL, LOCALE_SYSTEM_DEFAULT, DISPATCH_METHOD, &dp, &result, &exInfo, &n);
	if (FAILED(hr))
	{
		// Log detailed exception information
		std::wcerr << L"Invoke failed with error: " << hr << std::endl;
		std::wcerr << L"Exception code: " << exInfo.scode << std::endl;
		if (exInfo.bstrSource)
		{
			std::wcerr << L"Source: " << exInfo.bstrSource << std::endl;
			SysFreeString(exInfo.bstrSource);
		}
		if (exInfo.bstrDescription)
		{
			std::wcerr << L"Description: " << exInfo.bstrDescription << std::endl;
			SysFreeString(exInfo.bstrDescription);
		}
		if (exInfo.bstrHelpFile)
		{
			std::wcerr << L"Help file: " << exInfo.bstrHelpFile << std::endl;
			SysFreeString(exInfo.bstrHelpFile);
		}
		return hr;
	}

	return S_OK;
}


HRESULT CDMDriveShellView::CallMethod(LPCWSTR assemblyName, LPCWSTR className, LPCWSTR methodName, LONGLONG param)
{
	ICLRMetaHost* pMetaHost = nullptr;
	ICLRRuntimeInfo* pRuntimeInfo = nullptr;
	ICLRRuntimeHost* pClrRuntimeHost = nullptr;
	HRESULT hr = S_OK;

	// Start the CLR
	hr = CLRCreateInstance(CLSID_CLRMetaHost, IID_PPV_ARGS(&pMetaHost));
	if (FAILED(hr))
	{
		return hr;
	}

	hr = pMetaHost->GetRuntime(L"v4.0.30319", IID_PPV_ARGS(&pRuntimeInfo));
	if (FAILED(hr))
	{
		pMetaHost->Release();
		return hr;
	}

	BOOL fLoadable;
	hr = pRuntimeInfo->IsLoadable(&fLoadable);
	if (FAILED(hr) || !fLoadable)
	{
		pRuntimeInfo->Release();
		pMetaHost->Release();
		return hr;
	}

	hr = pRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_PPV_ARGS(&pClrRuntimeHost));
	if (FAILED(hr))
	{
		pRuntimeInfo->Release();
		pMetaHost->Release();
		return hr;
	}

	hr = pClrRuntimeHost->Start();
	if (FAILED(hr))
	{
		pClrRuntimeHost->Release();
		pRuntimeInfo->Release();
		pMetaHost->Release();
		return hr;
	}

	DWORD pReturnValue;
	wchar_t paramStr[20];
	swprintf_s(paramStr, 20, L"%lld", param);
	hr = pClrRuntimeHost->ExecuteInDefaultAppDomain(assemblyName, className, methodName, paramStr, &pReturnValue);

	pClrRuntimeHost->Release();
	pRuntimeInfo->Release();
	pMetaHost->Release();

	return hr;
}

//void CDMDriveShellView::LoadCDM(HWND hWnd, HWND hWndParent)
//{
//	// Combine HWND and HWNDParent into a single long parameter if necessary
//	// For simplicity, we'll just pass hWnd in this example
//
//	LONGLONG param = reinterpret_cast<LONGLONG>(hWnd);
//
//	HRESULT hr = CallMethod(L"CDMWrapper.dll", L"CDMWrapper.CDMWrapper", L"showCDM", param);
//	if (FAILED(hr))
//	{
//		// Handle error
//	}
//}


void CDMDriveShellView::LoadCDM(HWND hWnd, HWND hWndParent, HWND hWndLeft)
{
	// Create a unique pointer to a CCLRLoaderSimple object
	m_pCLRLoader = std::make_unique<CCLRLoaderSimple>();

	// Attempt to create an instance of the COM object with the specified ProgID
	HRESULT hr = m_pCLRLoader->CreateInstance(L"CDMWrapper", L"CDMWrapper.CDMWrapper", &m_cdmPtr);

	// If the instance creation failed, exit the function
	if (FAILED(hr))
	{
		// Log the error if logging is available
		// e.g., Logger::LogError(L"Failed to create instance of CDMWrapper");
		return;
	}

	// Prepare the parameters to be passed to the COM method
	CComVariant v1((UINT64)hWnd);
	CComVariant v2((UINT64)hWndParent);
	CComVariant v3((UINT64)hWndLeft);

	CComVariant result;

	// Call the "showCDM" method on the COM object with the prepared parameters
	hr = CallIDispatchMethod(m_cdmPtr, L"showCDM", v1, v2, v3, result);

	// If the method call failed, exit the function
	if (FAILED(hr))
	{
		// Log the error if logging is available
		// e.g., Logger::LogError(L"Failed to call showCDM method");
		return;
	}
}



IFACEMETHODIMP CDMDriveShellView::CreateViewWindow(_In_ LPSHELLVIEW pPrevious, _In_ LPCFOLDERSETTINGS pfs, _In_ LPSHELLBROWSER psb, _In_ LPRECT prcView, _Out_ HWND* phWnd)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"Creates a custom shell view");
		*phWnd = NULL;

		LOGINFO(_MainApplication->GetLogger(), L"Init member variables");
		m_pShellBrowser = psb;
		m_FolderSettings = *pfs;

		LOGINFO(_MainApplication->GetLogger(), L"Get the parent window from Explorer");
		m_pShellBrowser->GetWindow(&m_hwndParent);
		LOGINFO(_MainApplication->GetLogger(), L"The parent window handle is 0x%08X", m_hwndParent);

		LOGINFO(_MainApplication->GetLogger(), L"Create a container window");
		if (NULL == Create(m_hwndParent, *prcView))
			return E_FAIL;

		LOGINFO(_MainApplication->GetLogger(), L"Return our window handle to the browser.");
		*phWnd = m_hWnd;
		HWND leftPanelHwnd = GetFileExplorerLeftPanelHWND();
		if (leftPanelHwnd) {
			LoadCDM(m_hWnd, m_hwndParent, leftPanelHwnd);
		}

		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CDMDriveShellView::DestroyViewWindow()
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"Clean up the UI");
		UIActivate(SVUIA_DEACTIVATE);

		LOGINFO(_MainApplication->GetLogger(), L"Destroy the menu");
		if (NULL != m_hMenu)
			DestroyMenu(m_hMenu);

		LOGINFO(_MainApplication->GetLogger(), L"Destroy the window");
		DestroyWindow();
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CDMDriveShellView::GetCurrentInfo(_Out_ LPFOLDERSETTINGS pfs)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"GetCurrentInfo:");
		*pfs = m_FolderSettings;
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CDMDriveShellView::Refresh()
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"Refresh the window");
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CDMDriveShellView::UIActivate(_In_ UINT uState)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"UIActivate: uState = %d", uState);

		LOGINFO(_MainApplication->GetLogger(), L"Nothing to do if the state hasn't changed since the last call");
		if (m_uUIState == uState)
			return S_OK;

		LOGINFO(_MainApplication->GetLogger(), L"Modify the Explorer menu and status bar.");
		_HandleActivate(uState);

		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CDMDriveShellView::AddPropertySheetPages(_In_ DWORD dwReserved, _In_ LPFNSVADDPROPSHEETPAGE pfn, _In_ LPARAM lparam)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"AddPropertySheetPages:");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CDMDriveShellView::EnableModeless(_In_ BOOL fEnable)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"EnableModeless: fEnable = %d", fEnable);
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CDMDriveShellView::GetItemObject(_In_ UINT uItem, _In_ REFIID riid, _Out_ LPVOID* ppv)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"GetItemObject: uItem = %d", uItem);
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CDMDriveShellView::SaveViewState()
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"SaveViewState:");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CDMDriveShellView::SelectItem(_In_ PCUITEMID_CHILD pidlItem, _In_ SVSIF uFlags)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"SelectItem:");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CDMDriveShellView::TranslateAccelerator(LPMSG msg)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"TranslateAccelerator:");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CDMDriveShellView::QueryStatus(_In_ const GUID* pguidCmdGroup, _In_ ULONG cCmds, _In_ OLECMD prgCmds[], _Out_ OLECMDTEXT* pCmdText)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"QueryStatus:");

		if (NULL == prgCmds)
			return E_POINTER;

		// The only useful standard command I've figured out is "refresh".  I've put some trace messages in so you can see the other commands that the
		// browser sends our way. 

		if (NULL == pguidCmdGroup)
		{
			LOGINFO(_MainApplication->GetLogger(), L"guidCmdGroup is NULL");

			for (UINT u = 0; u < cCmds; u++)
			{
				LOGINFO(_MainApplication->GetLogger(), L"Query - DEFAULT: %u\n", prgCmds[u]);

				switch (prgCmds[u].cmdID)
				{
				case OLECMDID_REFRESH:
					prgCmds[u].cmdf = OLECMDF_SUPPORTED | OLECMDF_ENABLED;
					break;
				}
			}

			return S_OK;
		}
		else if (CGID_Explorer == *pguidCmdGroup)
		{
			LOGINFO(_MainApplication->GetLogger(), L"guidCmdGroup is CGID_Explorer");

			for (UINT u = 0; u < cCmds; u++)
			{
				LOGINFO(_MainApplication->GetLogger(), L"Query - EXPLORER: %u\n", prgCmds[u]);
			}
		}
		else if (CGID_ShellDocView == *pguidCmdGroup)
		{
			LOGINFO(_MainApplication->GetLogger(), L"guidCmdGroup is CGID_ShellDocView");
			for (UINT u = 0; u < cCmds; u++)
			{
				LOGINFO(_MainApplication->GetLogger(), L"Query - DOCVIEW: %u\n", prgCmds[u]);
			}
		}

		return OLECMDERR_E_UNKNOWNGROUP;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CDMDriveShellView::Exec(_In_ const GUID* pguidCmdGroup, _In_ DWORD nCmdID, _In_ DWORD nCmdexecopt, _In_ VARIANT* pvaIn, _Out_ VARIANT* pvaOut)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"Exec:");
		HRESULT hrRet = OLECMDERR_E_UNKNOWNGROUP;

		// The only standard command we act on is "refresh".  I've put some trace messages in so you can see the other commands that the
		// browser sends our way.

		if (NULL == pguidCmdGroup)
		{
			LOGINFO(_MainApplication->GetLogger(), L"guidCmdGroup is NULL");
			LOGINFO(_MainApplication->GetLogger(), L"Exec - DEFAULT: %u", nCmdID);

			if (OLECMDID_REFRESH == nCmdID)
			{
				Refresh();
				hrRet = S_OK;
			}
		}
		else if (CGID_Explorer == *pguidCmdGroup)
		{
			LOGINFO(_MainApplication->GetLogger(), L"guidCmdGroup is CGID_Explorer");
			LOGINFO(_MainApplication->GetLogger(), L"Exec - EXPLORER : %u", nCmdID);
		}
		else if (CGID_ShellDocView == *pguidCmdGroup)
		{
			LOGINFO(_MainApplication->GetLogger(), L"guidCmdGroup is CGID_ShellDocView");
			LOGINFO(_MainApplication->GetLogger(), L"Exec - DOCVIEW: %u", nCmdID);
		}

		return hrRet;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

LRESULT CDMDriveShellView::OnCreate(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"OnCreate:");
		HWND hHostWnd = _MainApplication->CreateViewHostInstance(m_hwndParent);

		if (hHostWnd != 0)
		{
			LOGINFO(_MainApplication->GetLogger(), L"lWnd is %x", hHostWnd);

			m_wndHost.Attach(hHostWnd);
			m_wndHost.SetParent(m_hWnd);
			if (!m_wndHost.IsWindowVisible())
			{
				m_wndHost.ShowWindow(SW_SHOW);
			}
		}

		return 0;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return (-1);
	}
}

LRESULT CDMDriveShellView::OnSize(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"Resize the list control to the same size as the container window.");

		if (m_wndHost.IsWindow())
			m_wndHost.MoveWindow(0, 0, LOWORD(lParam), HIWORD(lParam), TRUE);

		return 0;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return (-1);
	}
}

LRESULT CDMDriveShellView::OnSetFocus(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"set the focus to the list control.");
		// This handler is called when the list container window gets the focus, 
		// usually because the user tabbed to it.  Immediately set the focus to
		// the list control.

		if (m_wndHost.IsWindow())
			m_wndHost.SetFocus();
		return 0;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return (-1);
	}
}

LRESULT CDMDriveShellView::OnContextMenu(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"OnContextMenu:");
		return 0;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return (-1);
	}
}

void CDMDriveShellView::Init(CComPtr<IShellFolder>& pContainingFolder)
{
	m_pContainingFolder = pContainingFolder;
}


void CDMDriveShellView::_HandleActivate(UINT uState)
{
	LOGINFO(_MainApplication->GetLogger(), L"Undo our previous changes to the menu");
	_HandleDeactivate();

	LOGINFO(_MainApplication->GetLogger(), L"If we are being activated, add our stuff to Explorer's menu");
	if (SVUIA_DEACTIVATE != uState)
	{
		LOGINFO(_MainApplication->GetLogger(), L"create a new menu");

		LOGINFO(_MainApplication->GetLogger(), L"Modify the status bar");
		m_pShellBrowser->SetStatusTextSB(L"Assetmax CRM Namespace Extension");
	}

	LOGINFO(_MainApplication->GetLogger(), L"Save the current state");
	m_uUIState = uState;
}

void CDMDriveShellView::_HandleDeactivate()
{
	if (SVUIA_DEACTIVATE != m_uUIState)
	{
		LOGINFO(_MainApplication->GetLogger(), L"Deactivate menu and status bar");
		if (NULL != m_hMenu)
		{
			m_pShellBrowser->SetMenuSB(NULL, NULL, NULL);
			m_pShellBrowser->RemoveMenusSB(m_hMenu);

			LOGINFO(_MainApplication->GetLogger(), L"also destroys the SimpleNSExt submenu");
			DestroyMenu(m_hMenu);
			m_hMenu = NULL;
		}

		m_uUIState = SVUIA_DEACTIVATE;
	}
}
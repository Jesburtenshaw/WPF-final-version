#include "pch.h"
#include "application.h"

CApplicationSingleton _MainApplication;

static LPCWSTR szPluginAssemblyName = L"cdmdrive.plugin";
static LPCWSTR szPluginClassName = L"cdmdrive.plugin.ComPlugin";

CApplication::CApplication() 
	: m_bIsInitialized(FALSE)
{
	m_pLogger = auto_ptr<utilities::diagnostics::CLogger<utilities::threading::CIntraProcessLock>>(new utilities::diagnostics::CLogger<utilities::threading::CIntraProcessLock>(
		utilities::diagnostics::log_level::Info, _T("cdmdrive-shell-namespace")));

	m_pLogger->AddOutputStream(new std::wofstream(BuildLogFileName()), true);
	LOGINFO(m_pLogger, L"Initialized the Logger");

	LOGINFO(m_pLogger, L"Creates the CLR Loader");
	m_pCLRLoader = auto_ptr<CCLRLoader>(new (std::nothrow)CCLRLoader());
}

CApplication::~CApplication()
{
}

void CApplication::Initialize()
{
	if (m_bIsInitialized)
		return;

	LOGINFO(m_pLogger, L"CApplication::Initialize called");
	m_bIsInitialized = TRUE;
	InitilaizeShellPlugin();
}

void CApplication::Done()
{
	if (!m_bIsInitialized)
		return;

	// deinitializes the logger
	LOGINFO(m_pLogger, L"CApplication::Done called");
	if (m_pShellPlugin.p != NULL)
	{
		LOGINFO(m_pLogger, L"CApplication::Done - done shellplugin");
		m_pShellPlugin = NULL;
		LOGINFO(m_pLogger, L"CApplication::Done - unload CRL");
		m_pCLRLoader->Unload();
		LOGINFO(m_pLogger, L"CApplication::Done finished");
	}
}

HWND CApplication::CreateViewHostInstance(HWND hwndOwner)
{
	if (m_pShellPlugin.p == nullptr)
		return nullptr;

	HWND hHostWnd = 0;
	return utilities::error_signaling::helper::check_hresult(m_pShellPlugin->GetRootWindow(hwndOwner, &hHostWnd)) ? hHostWnd : nullptr;
}

void CApplication::InitilaizeShellPlugin()
{
	HRESULT hr = m_pCLRLoader->CreateInstance(szPluginAssemblyName, szPluginClassName, __uuidof(ICDMShellPlugin), reinterpret_cast<void**>(&m_pShellPlugin));
	LOGINFO(m_pLogger, L"The %s assembly was loaded and class %s was created %s", szPluginAssemblyName, szPluginClassName, FAILED(hr) ? L"failed" : L"successfully");
	if (m_pShellPlugin.p != NULL)
	{
		hr = m_pShellPlugin->Init();
		LOGINFO(m_pLogger, L"The ShellPlugin was initialized %s", FAILED(hr) ? L"failed" : L"successfully");
	}
}

tstring CApplication::BuildLogFileName()
{
	WCHAR buffer[256];
	time_t szClock;
	tm newTime;

	time(&szClock);
	localtime_s(&newTime, &szClock);

	wcsftime(buffer, sizeof(buffer), L"addin-%d-%m-%Y-%H-%M-%S.log", &newTime);
	tstring addinName(buffer);

	tstring logFileName = utilities::path_info::get_dll_folder(_AtlBaseModule.GetModuleInstance());
	logFileName = utilities::path_info::combine_path(logFileName, addinName);
	
	return logFileName;
}

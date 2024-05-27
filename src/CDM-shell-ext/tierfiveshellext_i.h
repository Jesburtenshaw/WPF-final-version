

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.01.0628 */
/* at Tue Jan 19 08:44:07 2038
 */
/* Compiler settings for tierfiveshellext.idl:
    Oicf, W1, Zp8, env=Win64 (32b run), target_arch=AMD64 8.01.0628 
    protocol : all , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */



/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 500
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif /* __RPCNDR_H_VERSION__ */

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __tierfiveshellext_i_h__
#define __tierfiveshellext_i_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

#ifndef DECLSPEC_XFGVIRT
#if defined(_CONTROL_FLOW_GUARD_XFG)
#define DECLSPEC_XFGVIRT(base, func) __declspec(xfg_virtual(base, func))
#else
#define DECLSPEC_XFGVIRT(base, func)
#endif
#endif

/* Forward Declarations */ 

#ifndef __IFSShellPlugin_FWD_DEFINED__
#define __IFSShellPlugin_FWD_DEFINED__
typedef interface IFSShellPlugin IFSShellPlugin;

#endif 	/* __IFSShellPlugin_FWD_DEFINED__ */


#ifndef __ICfsDriveShellExt_FWD_DEFINED__
#define __ICfsDriveShellExt_FWD_DEFINED__
typedef interface ICfsDriveShellExt ICfsDriveShellExt;

#endif 	/* __ICfsDriveShellExt_FWD_DEFINED__ */


#ifndef __ICfsDriveShellView_FWD_DEFINED__
#define __ICfsDriveShellView_FWD_DEFINED__
typedef interface ICfsDriveShellView ICfsDriveShellView;

#endif 	/* __ICfsDriveShellView_FWD_DEFINED__ */


#ifndef __CfsDriveShellExt_FWD_DEFINED__
#define __CfsDriveShellExt_FWD_DEFINED__

#ifdef __cplusplus
typedef class CfsDriveShellExt CfsDriveShellExt;
#else
typedef struct CfsDriveShellExt CfsDriveShellExt;
#endif /* __cplusplus */

#endif 	/* __CfsDriveShellExt_FWD_DEFINED__ */


#ifndef __FsDriveShellView_FWD_DEFINED__
#define __FsDriveShellView_FWD_DEFINED__

#ifdef __cplusplus
typedef class FsDriveShellView FsDriveShellView;
#else
typedef struct FsDriveShellView FsDriveShellView;
#endif /* __cplusplus */

#endif 	/* __FsDriveShellView_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"
#include "shobjidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __IFSShellPlugin_INTERFACE_DEFINED__
#define __IFSShellPlugin_INTERFACE_DEFINED__

/* interface IFSShellPlugin */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IFSShellPlugin;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("AC80FE12-31FD-4374-9E71-A6ED7D605FA0")
    IFSShellPlugin : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Init( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Done( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetRootWindow( 
            /* [in] */ HWND hwndOwner,
            /* [retval][out] */ HWND *phwnd) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IFSShellPluginVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IFSShellPlugin * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IFSShellPlugin * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IFSShellPlugin * This);
        
        DECLSPEC_XFGVIRT(IDispatch, GetTypeInfoCount)
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IFSShellPlugin * This,
            /* [out] */ UINT *pctinfo);
        
        DECLSPEC_XFGVIRT(IDispatch, GetTypeInfo)
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IFSShellPlugin * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        DECLSPEC_XFGVIRT(IDispatch, GetIDsOfNames)
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IFSShellPlugin * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        DECLSPEC_XFGVIRT(IDispatch, Invoke)
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IFSShellPlugin * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        DECLSPEC_XFGVIRT(IFSShellPlugin, Init)
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Init )( 
            IFSShellPlugin * This);
        
        DECLSPEC_XFGVIRT(IFSShellPlugin, Done)
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Done )( 
            IFSShellPlugin * This);
        
        DECLSPEC_XFGVIRT(IFSShellPlugin, GetRootWindow)
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetRootWindow )( 
            IFSShellPlugin * This,
            /* [in] */ HWND hwndOwner,
            /* [retval][out] */ HWND *phwnd);
        
        END_INTERFACE
    } IFSShellPluginVtbl;

    interface IFSShellPlugin
    {
        CONST_VTBL struct IFSShellPluginVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IFSShellPlugin_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IFSShellPlugin_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IFSShellPlugin_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IFSShellPlugin_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IFSShellPlugin_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IFSShellPlugin_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IFSShellPlugin_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IFSShellPlugin_Init(This)	\
    ( (This)->lpVtbl -> Init(This) ) 

#define IFSShellPlugin_Done(This)	\
    ( (This)->lpVtbl -> Done(This) ) 

#define IFSShellPlugin_GetRootWindow(This,hwndOwner,phwnd)	\
    ( (This)->lpVtbl -> GetRootWindow(This,hwndOwner,phwnd) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IFSShellPlugin_INTERFACE_DEFINED__ */


#ifndef __ICfsDriveShellExt_INTERFACE_DEFINED__
#define __ICfsDriveShellExt_INTERFACE_DEFINED__

/* interface ICfsDriveShellExt */
/* [unique][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ICfsDriveShellExt;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("b203cd4b-9f89-4949-9aa2-8ffe7ac02de9")
    ICfsDriveShellExt : public IUnknown
    {
    public:
    };
    
    
#else 	/* C style interface */

    typedef struct ICfsDriveShellExtVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ICfsDriveShellExt * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ICfsDriveShellExt * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ICfsDriveShellExt * This);
        
        END_INTERFACE
    } ICfsDriveShellExtVtbl;

    interface ICfsDriveShellExt
    {
        CONST_VTBL struct ICfsDriveShellExtVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ICfsDriveShellExt_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ICfsDriveShellExt_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ICfsDriveShellExt_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ICfsDriveShellExt_INTERFACE_DEFINED__ */


#ifndef __ICfsDriveShellView_INTERFACE_DEFINED__
#define __ICfsDriveShellView_INTERFACE_DEFINED__

/* interface ICfsDriveShellView */
/* [unique][helpstring][uuid][object] */ 


EXTERN_C const IID IID_ICfsDriveShellView;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("252BA8FF-F9DC-4758-B72C-17C05D53BC72")
    ICfsDriveShellView : public IUnknown
    {
    public:
    };
    
    
#else 	/* C style interface */

    typedef struct ICfsDriveShellViewVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ICfsDriveShellView * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ICfsDriveShellView * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ICfsDriveShellView * This);
        
        END_INTERFACE
    } ICfsDriveShellViewVtbl;

    interface ICfsDriveShellView
    {
        CONST_VTBL struct ICfsDriveShellViewVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ICfsDriveShellView_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ICfsDriveShellView_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ICfsDriveShellView_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ICfsDriveShellView_INTERFACE_DEFINED__ */



#ifndef __tierfiveshellextLib_LIBRARY_DEFINED__
#define __tierfiveshellextLib_LIBRARY_DEFINED__

/* library tierfiveshellextLib */
/* [version][uuid] */ 


EXTERN_C const IID LIBID_tierfiveshellextLib;

EXTERN_C const CLSID CLSID_CfsDriveShellExt;

#ifdef __cplusplus

class DECLSPEC_UUID("f70a8770-1f4b-4af5-90e4-35260bcd97df")
CfsDriveShellExt;
#endif

EXTERN_C const CLSID CLSID_FsDriveShellView;

#ifdef __cplusplus

class DECLSPEC_UUID("61963ADC-3165-404C-9381-32C21CD3C754")
FsDriveShellView;
#endif
#endif /* __tierfiveshellextLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  HWND_UserSize(     unsigned long *, unsigned long            , HWND * ); 
unsigned char * __RPC_USER  HWND_UserMarshal(  unsigned long *, unsigned char *, HWND * ); 
unsigned char * __RPC_USER  HWND_UserUnmarshal(unsigned long *, unsigned char *, HWND * ); 
void                      __RPC_USER  HWND_UserFree(     unsigned long *, HWND * ); 

unsigned long             __RPC_USER  HWND_UserSize64(     unsigned long *, unsigned long            , HWND * ); 
unsigned char * __RPC_USER  HWND_UserMarshal64(  unsigned long *, unsigned char *, HWND * ); 
unsigned char * __RPC_USER  HWND_UserUnmarshal64(unsigned long *, unsigned char *, HWND * ); 
void                      __RPC_USER  HWND_UserFree64(     unsigned long *, HWND * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif





/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.01.0628 */
/* at Tue Jan 19 08:44:07 2038
 */
/* Compiler settings for CDMShellExt.idl:
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

#ifndef __CDMShellExt_i_h__
#define __CDMShellExt_i_h__

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

#ifndef __ICDMShellPlugin_FWD_DEFINED__
#define __ICDMShellPlugin_FWD_DEFINED__
typedef interface ICDMShellPlugin ICDMShellPlugin;

#endif 	/* __ICDMShellPlugin_FWD_DEFINED__ */


#ifndef __ICDMDriveShellExt_FWD_DEFINED__
#define __ICDMDriveShellExt_FWD_DEFINED__
typedef interface ICDMDriveShellExt ICDMDriveShellExt;

#endif 	/* __ICDMDriveShellExt_FWD_DEFINED__ */


#ifndef __ICDMDriveShellView_FWD_DEFINED__
#define __ICDMDriveShellView_FWD_DEFINED__
typedef interface ICDMDriveShellView ICDMDriveShellView;

#endif 	/* __ICDMDriveShellView_FWD_DEFINED__ */


#ifndef __CDMDriveShellExt_FWD_DEFINED__
#define __CDMDriveShellExt_FWD_DEFINED__

#ifdef __cplusplus
typedef class CDMDriveShellExt CDMDriveShellExt;
#else
typedef struct CDMDriveShellExt CDMDriveShellExt;
#endif /* __cplusplus */

#endif 	/* __CDMDriveShellExt_FWD_DEFINED__ */


#ifndef __CDMDriveShellView_FWD_DEFINED__
#define __CDMDriveShellView_FWD_DEFINED__

#ifdef __cplusplus
typedef class CDMDriveShellView CDMDriveShellView;
#else
typedef struct CDMDriveShellView CDMDriveShellView;
#endif /* __cplusplus */

#endif 	/* __CDMDriveShellView_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"
#include "shobjidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __ICDMShellPlugin_INTERFACE_DEFINED__
#define __ICDMShellPlugin_INTERFACE_DEFINED__

/* interface ICDMShellPlugin */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ICDMShellPlugin;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("AC80FE12-31FD-4374-9E71-A6ED7D605FA0")
    ICDMShellPlugin : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Init( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Done( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetRootWindow( 
            /* [in] */ HWND hwndOwner,
            /* [retval][out] */ HWND *phwnd) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct ICDMShellPluginVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ICDMShellPlugin * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ICDMShellPlugin * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ICDMShellPlugin * This);
        
        DECLSPEC_XFGVIRT(IDispatch, GetTypeInfoCount)
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ICDMShellPlugin * This,
            /* [out] */ UINT *pctinfo);
        
        DECLSPEC_XFGVIRT(IDispatch, GetTypeInfo)
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ICDMShellPlugin * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        DECLSPEC_XFGVIRT(IDispatch, GetIDsOfNames)
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ICDMShellPlugin * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        DECLSPEC_XFGVIRT(IDispatch, Invoke)
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ICDMShellPlugin * This,
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
        
        DECLSPEC_XFGVIRT(ICDMShellPlugin, Init)
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Init )( 
            ICDMShellPlugin * This);
        
        DECLSPEC_XFGVIRT(ICDMShellPlugin, Done)
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Done )( 
            ICDMShellPlugin * This);
        
        DECLSPEC_XFGVIRT(ICDMShellPlugin, GetRootWindow)
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetRootWindow )( 
            ICDMShellPlugin * This,
            /* [in] */ HWND hwndOwner,
            /* [retval][out] */ HWND *phwnd);
        
        END_INTERFACE
    } ICDMShellPluginVtbl;

    interface ICDMShellPlugin
    {
        CONST_VTBL struct ICDMShellPluginVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ICDMShellPlugin_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ICDMShellPlugin_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ICDMShellPlugin_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ICDMShellPlugin_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ICDMShellPlugin_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ICDMShellPlugin_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ICDMShellPlugin_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ICDMShellPlugin_Init(This)	\
    ( (This)->lpVtbl -> Init(This) ) 

#define ICDMShellPlugin_Done(This)	\
    ( (This)->lpVtbl -> Done(This) ) 

#define ICDMShellPlugin_GetRootWindow(This,hwndOwner,phwnd)	\
    ( (This)->lpVtbl -> GetRootWindow(This,hwndOwner,phwnd) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ICDMShellPlugin_INTERFACE_DEFINED__ */


#ifndef __ICDMDriveShellExt_INTERFACE_DEFINED__
#define __ICDMDriveShellExt_INTERFACE_DEFINED__

/* interface ICDMDriveShellExt */
/* [unique][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ICDMDriveShellExt;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("b203cd4b-9f89-4949-9aa2-8ffe7ac02de9")
    ICDMDriveShellExt : public IUnknown
    {
    public:
    };
    
    
#else 	/* C style interface */

    typedef struct ICDMDriveShellExtVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ICDMDriveShellExt * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ICDMDriveShellExt * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ICDMDriveShellExt * This);
        
        END_INTERFACE
    } ICDMDriveShellExtVtbl;

    interface ICDMDriveShellExt
    {
        CONST_VTBL struct ICDMDriveShellExtVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ICDMDriveShellExt_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ICDMDriveShellExt_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ICDMDriveShellExt_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ICDMDriveShellExt_INTERFACE_DEFINED__ */


#ifndef __ICDMDriveShellView_INTERFACE_DEFINED__
#define __ICDMDriveShellView_INTERFACE_DEFINED__

/* interface ICDMDriveShellView */
/* [unique][helpstring][uuid][object] */ 


EXTERN_C const IID IID_ICDMDriveShellView;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("252BA8FF-F9DC-4758-B72C-17C05D53BC72")
    ICDMDriveShellView : public IUnknown
    {
    public:
    };
    
    
#else 	/* C style interface */

    typedef struct ICDMDriveShellViewVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ICDMDriveShellView * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ICDMDriveShellView * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ICDMDriveShellView * This);
        
        END_INTERFACE
    } ICDMDriveShellViewVtbl;

    interface ICDMDriveShellView
    {
        CONST_VTBL struct ICDMDriveShellViewVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ICDMDriveShellView_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ICDMDriveShellView_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ICDMDriveShellView_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ICDMDriveShellView_INTERFACE_DEFINED__ */



#ifndef __CDMShellExtLib_LIBRARY_DEFINED__
#define __CDMShellExtLib_LIBRARY_DEFINED__

/* library CDMShellExtLib */
/* [version][uuid] */ 


EXTERN_C const IID LIBID_CDMShellExtLib;

EXTERN_C const CLSID CLSID_CDMDriveShellExt;

#ifdef __cplusplus

class DECLSPEC_UUID("f70a8770-1f4b-4af5-90e4-35260bcd97df")
CDMDriveShellExt;
#endif

EXTERN_C const CLSID CLSID_CDMDriveShellView;

#ifdef __cplusplus

class DECLSPEC_UUID("61963ADC-3165-404C-9381-32C21CD3C754")
CDMDriveShellView;
#endif
#endif /* __CDMShellExtLib_LIBRARY_DEFINED__ */

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



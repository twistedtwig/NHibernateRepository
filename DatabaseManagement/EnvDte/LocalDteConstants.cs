using System.Runtime.InteropServices;

namespace DatabaseManagement.EnvDte
{
    internal class LocalDteConstants
    {
        [MarshalAs(20)]
        internal const string vsDocumentKindText = "{8E7B96A8-E33D-11D0-A6D5-00C04FB67F6A}";
        [MarshalAs(20)]
        internal const string vsDocumentKindHTML = "{C76D83F8-A489-11D0-8195-00A0C91BBEE3}";
        [MarshalAs(20)]
        internal const string vsDocumentKindResource = "{00000000-0000-0000-0000-000000000000}";
        [MarshalAs(20)]
        internal const string vsDocumentKindBinary = "{25834150-CD7E-11D0-92DF-00A0C9138C45}";
        [MarshalAs(20)]
        internal const string vsViewKindPrimary = "{00000000-0000-0000-0000-000000000000}";
        [MarshalAs(20)]
        internal const string vsViewKindAny = "{FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF}";
        [MarshalAs(20)]
        internal const string vsViewKindDebugging = "{7651A700-06E5-11D1-8EBD-00A0C90F26EA}";
        [MarshalAs(20)]
        internal const string vsViewKindCode = "{7651A701-06E5-11D1-8EBD-00A0C90F26EA}";
        [MarshalAs(20)]
        internal const string vsViewKindDesigner = "{7651A702-06E5-11D1-8EBD-00A0C90F26EA}";
        [MarshalAs(20)]
        internal const string vsViewKindTextView = "{7651A703-06E5-11D1-8EBD-00A0C90F26EA}";
        [MarshalAs(20)]
        internal const string vsWindowKindTaskList = "{4A9B7E51-AA16-11D0-A8C5-00A0C921A4D2}";
        [MarshalAs(20)]
        internal const string vsWindowKindToolbox = "{B1E99781-AB81-11D0-B683-00AA00A3EE26}";
        [MarshalAs(20)]
        internal const string vsWindowKindCallStack = "{0504FF91-9D61-11D0-A794-00A0C9110051}";
        [MarshalAs(20)]
        internal const string vsWindowKindThread = "{E62CE6A0-B439-11D0-A79D-00A0C9110051}";
        [MarshalAs(20)]
        internal const string vsWindowKindLocals = "{4A18F9D0-B838-11D0-93EB-00A0C90F2734}";
        [MarshalAs(20)]
        internal const string vsWindowKindAutoLocals = "{F2E84780-2AF1-11D1-A7FA-00A0C9110051}";
        [MarshalAs(20)]
        internal const string vsWindowKindWatch = "{90243340-BD7A-11D0-93EF-00A0C90F2734}";
        [MarshalAs(20)]
        internal const string vsWindowKindProperties = "{EEFA5220-E298-11D0-8F78-00A0C9110057}";
        [MarshalAs(20)]
        internal const string vsWindowKindSolutionExplorer = "{3AE79031-E1BC-11D0-8F78-00A0C9110057}";
        [MarshalAs(20)]
        internal const string vsWindowKindOutput = "{34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3}";
        [MarshalAs(20)]
        internal const string vsWindowKindObjectBrowser = "{269A02DC-6AF8-11D3-BDC4-00C04F688E50}";
        [MarshalAs(20)]
        internal const string vsWindowKindMacroExplorer = "{07CD18B4-3BA1-11D2-890A-0060083196C6}";
        [MarshalAs(20)]
        internal const string vsWindowKindDynamicHelp = "{66DBA47C-61DF-11D2-AA79-00C04F990343}";
        [MarshalAs(20)]
        internal const string vsWindowKindClassView = "{C9C0AE26-AA77-11D2-B3F0-0000F87570EE}";
        [MarshalAs(20)]
        internal const string vsWindowKindResourceView = "{2D7728C2-DE0A-45b5-99AA-89B609DFDE73}";
        [MarshalAs(20)]
        internal const string vsWindowKindDocumentOutline = "{25F7E850-FFA1-11D0-B63F-00A0C922E851}";
        [MarshalAs(20)]
        internal const string vsWindowKindServerExplorer = "{74946827-37A0-11D2-A273-00C04F8EF4FF}";
        [MarshalAs(20)]
        internal const string vsWindowKindCommandWindow = "{28836128-FC2C-11D2-A433-00C04F72D18A}";
        [MarshalAs(20)]
        internal const string vsWindowKindFindSymbol = "{53024D34-0EF5-11D3-87E0-00C04F7971A5}";
        [MarshalAs(20)]
        internal const string vsWindowKindFindSymbolResults = "{68487888-204A-11D3-87EB-00C04F7971A5}";
        [MarshalAs(20)]
        internal const string vsWindowKindFindReplace = "{CF2DDC32-8CAD-11D2-9302-005345000000}";
        [MarshalAs(20)]
        internal const string vsWindowKindFindResults1 = "{0F887920-C2B6-11D2-9375-0080C747D9A0}";
        [MarshalAs(20)]
        internal const string vsWindowKindFindResults2 = "{0F887921-C2B6-11D2-9375-0080C747D9A0}";
        [MarshalAs(20)]
        internal const string vsWindowKindMainWindow = "{9DDABE98-1D02-11D3-89A1-00C04F688DDE}";
        [MarshalAs(20)]
        internal const string vsWindowKindLinkedWindowFrame = "{9DDABE99-1D02-11D3-89A1-00C04F688DDE}";
        [MarshalAs(20)]
        internal const string vsWindowKindWebBrowser = "{E8B06F52-6D01-11D2-AA7D-00C04F990343}";
        [MarshalAs(20)]
        internal const string vsWizardAddSubProject = "{0F90E1D2-4999-11D1-B6D1-00A0C90F2744}";
        [MarshalAs(20)]
        internal const string vsWizardAddItem = "{0F90E1D1-4999-11D1-B6D1-00A0C90F2744}";
        [MarshalAs(20)]
        internal const string vsWizardNewProject = "{0F90E1D0-4999-11D1-B6D1-00A0C90F2744}";
        [MarshalAs(20)]
        internal const string vsProjectKindMisc = "{66A2671D-8FB5-11D2-AA7E-00C04F688DDE}";
        [MarshalAs(20)]
        internal const string vsProjectItemsKindMisc = "{66A2671E-8FB5-11D2-AA7E-00C04F688DDE}";
        [MarshalAs(20)]
        internal const string vsProjectItemKindMisc = "{66A2671F-8FB5-11D2-AA7E-00C04F688DDE}";
        [MarshalAs(20)]
        internal const string vsProjectKindUnmodeled = "{67294A52-A4F0-11D2-AA88-00C04F688DDE}";
        [MarshalAs(20)]
        internal const string vsProjectKindSolutionItems = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";
        [MarshalAs(20)]
        internal const string vsProjectItemsKindSolutionItems = "{66A26721-8FB5-11D2-AA7E-00C04F688DDE}";
        [MarshalAs(20)]
        internal const string vsProjectItemKindSolutionItems = "{66A26722-8FB5-11D2-AA7E-00C04F688DDE}";
        [MarshalAs(20)]
        internal const string vsProjectsKindSolution = "{96410B9F-3542-4A14-877F-BC7227B51D3B}";
        [MarshalAs(20)]
        internal const string vsAddInCmdGroup = "{1E58696E-C90F-11D2-AAB2-00C04F688DDE}";
        [MarshalAs(20)]
        internal const string vsContextSolutionBuilding = "{ADFC4E60-0397-11D1-9F4E-00A0C911004F}";
        [MarshalAs(20)]
        internal const string vsContextDebugging = "{ADFC4E61-0397-11D1-9F4E-00A0C911004F}";
        [MarshalAs(20)]
        internal const string vsContextFullScreenMode = "{ADFC4E62-0397-11D1-9F4E-00A0C911004F}";
        [MarshalAs(20)]
        internal const string vsContextDesignMode = "{ADFC4E63-0397-11D1-9F4E-00A0C911004F}";
        [MarshalAs(20)]
        internal const string vsContextNoSolution = "{ADFC4E64-0397-11D1-9F4E-00A0C911004F}";
        [MarshalAs(20)]
        internal const string vsContextEmptySolution = "{ADFC4E65-0397-11D1-9F4E-00A0C911004F}";
        [MarshalAs(20)]
        internal const string vsContextSolutionHasSingleProject = "{ADFC4E66-0397-11D1-9F4E-00A0C911004F}";
        [MarshalAs(20)]
        internal const string vsContextSolutionHasMultipleProjects = "{93694FA0-0397-11D1-9F4E-00A0C911004F}";
        [MarshalAs(20)]
        internal const string vsContextMacroRecording = "{04BBF6A5-4697-11D2-890E-0060083196C6}";
        [MarshalAs(20)]
        internal const string vsContextMacroRecordingToolbar = "{85A70471-270A-11D2-88F9-0060083196C6}";
        [MarshalAs(20)]
        internal const string vsMiscFilesProjectUniqueName = "<MiscFiles>";
        [MarshalAs(20)]
        internal const string vsSolutionItemsProjectUniqueName = "<SolnItems>";
        [MarshalAs(20)]
        internal const string vsProjectItemKindPhysicalFile = "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}";
        [MarshalAs(20)]
        internal const string vsProjectItemKindPhysicalFolder = "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";
        [MarshalAs(20)]
        internal const string vsProjectItemKindVirtualFolder = "{6BB5F8F0-4483-11D3-8BCF-00C04F8EC28C}";
        [MarshalAs(20)]
        internal const string vsProjectItemKindSubProject = "{EA6618E8-6E24-4528-94BE-6889FE16485C}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_vk_Primary = "{00000000-0000-0000-0000-000000000000}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_vk_Debugging = "{7651A700-06E5-11D1-8EBD-00A0C90F26EA}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_vk_Code = "{7651A701-06E5-11D1-8EBD-00A0C90F26EA}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_vk_Designer = "{7651A702-06E5-11D1-8EBD-00A0C90F26EA}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_vk_TextView = "{7651A703-06E5-11D1-8EBD-00A0C90F26EA}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_wk_TaskList = "{4A9B7E51-AA16-11D0-A8C5-00A0C921A4D2}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_wk_Toolbox = "{B1E99781-AB81-11D0-B683-00AA00A3EE26}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_wk_CallStackWindow = "{0504FF91-9D61-11D0-A794-00A0C9110051}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_wk_ThreadWindow = "{E62CE6A0-B439-11D0-A79D-00A0C9110051}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_wk_LocalsWindow = "{4A18F9D0-B838-11D0-93EB-00A0C90F2734}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_wk_AutoLocalsWindow = "{F2E84780-2AF1-11D1-A7FA-00A0C9110051}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_wk_WatchWindow = "{90243340-BD7A-11D0-93EF-00A0C90F2734}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_wk_ImmedWindow = "{98731960-965C-11D0-A78F-00A0C9110051}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_wk_PropertyBrowser = "{EEFA5220-E298-11D0-8F78-00A0C9110057}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_wk_SProjectWindow = "{3AE79031-E1BC-11D0-8F78-00A0C9110057}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_wk_OutputWindow = "{34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_wk_ObjectBrowser = "{269A02DC-6AF8-11D3-BDC4-00C04F688E50}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_wk_ContextWindow = "{66DBA47C-61DF-11D2-AA79-00C04F990343}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_wk_ClassView = "{C9C0AE26-AA77-11D2-B3F0-0000F87570EE}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_GUID_AddItemWizard = "{0F90E1D1-4999-11D1-B6D1-00A0C90F2744}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string vsext_GUID_NewProjectWizard = "{0F90E1D0-4999-11D1-B6D1-00A0C90F2744}";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string dsCPP = "C/C++";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string dsHTML_IE3 = "HTML - IE 3.0";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string dsHTML_RFC1866 = "HTML 2.0 (RFC 1866)";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string dsFortran_Fixed = "Fortran Fixed";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string dsFortran_Free = "Fortran Free";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string dsJava = "Java";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string dsVBSMacro = "VBS Macro";
        [TypeLibVar(64)]
        [MarshalAs(20)]
        internal const string dsIDL = "ODL/IDL";
        [MarshalAs(20)]
        internal const string vsCATIDSolution = "{52AEFF70-BBD8-11d2-8598-006097C68E81}";
        [MarshalAs(20)]
        internal const string vsCATIDSolutionBrowseObject = "{A2392464-7C22-11d3-BDCA-00C04F688E50}";
        [MarshalAs(20)]
        internal const string vsCATIDMiscFilesProject = "{610d4612-d0d5-11d2-8599-006097c68e81}";
        [MarshalAs(20)]
        internal const string vsCATIDMiscFilesProjectItem = "{610d4613-d0d5-11d2-8599-006097c68e81}";
        [MarshalAs(20)]
        internal const string vsCATIDGenericProject = "{610d4616-d0d5-11d2-8599-006097c68e81}";
        [MarshalAs(20)]
        internal const string vsCATIDDocument = "{610d4611-d0d5-11d2-8599-006097c68e81}";
    }
}

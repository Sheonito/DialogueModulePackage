# Changelog

## [0.1.0] - 2025-01-24
### Added
- Package Init

## [0.1.1] - 2025-01-31
### Modified
- Package sample namespace modified

## [0.1.2] - 2025-01-31
### Modified
- Package sample namespace modified

## [0.1.3] - 2025-01-31
### Modified
- TransitionsPlus plugin reimported

## [0.1.4] - 2025-01-31
### Modified
- Code generate storyData path automatically added
- TransitionPlus plugin reimported
- Package author name changed

## [0.1.5] - 2025-01-31
### Modified
- Way to create function class is changed to reflection using generic
- Type of FunctionType is changed to string

## [0.1.6] - 2025-01-31
### Modified
- StoryContent virtual method for registering functionEvent added

## [0.1.7] - 2025-01-31
### Modified
- Unused namespace removed
- FunctionDic key is changed to the key without namespace

## [0.1.8] - 2025-02-07
### Added
- Sample Save Load Scripts added
- Sample Scene updated about save load 

## [0.1.9] - 2025-02-07
### Fix
- Sample Save Load Scripts bug fixed 

## [0.1.10] - 2025-02-07
### Fix
- Sample Save Load Scripts bug fixed 

## [0.1.11] - 2025-02-10
### Added
- Plugin editor code assembly definition added 

## [0.1.12] - 2025-02-10
### Added
- RubyTextMeshPro Plugin added

## [0.1.13] - 2025-02-10
### Added
- Unity Json,Localization dependencies added

## [0.2.0] - 2025-02-11
### Added
- Sample,Module downloader added

## [0.2.1] - 2025-02-12
### Fix
- SaveVersion error fixed

## [0.2.2] - 2025-02-12
### Modified
- NavigationGroup in view added  
- IContentView deleted
- Popup changed to generic
- Layout added

## [0.2.3] - 2025-02-17
### Modified
- Some LayoutBase functions modified to virtual

## [0.2.4] - 2025-02-17
### Added
- TimelineDirector MoveCurrentTimelineTime function Added

## [0.2.5] - 2025-02-18
### Modified
- StoryContent null comparision code 
- added

## [0.2.6] - 2025-02-18
### Added
- Transition plugin added

## [0.2.7] - 2025-02-19
### Fixed
- Content InjectDependency fixed

## [0.2.8] - 2025-02-19
### Deleted
- ConversationEndPoint deleted

## [0.3.0] - 2025-02-20
### Deleted
- TransitionDirector error fixed
- BackgroundView changed to BackgroundStage
- Character_showObject,Character_overlapObject added
- FunctionInConversation command added. ex) Hold me baby{#Character_show=neko} uki uki lady.

## [0.3.1] - 2025-02-22
### Added
- FunctionInConversation when conversation skipped added
- LayoutBase IsOn variable added

## [0.3.2] - 2025-02-26
### Added
- Skip,backlog,auto selectable added to foregroundView
- Vignette shader and function added

## [0.3.3] - 2025-02-27
### Added
- Eyes open,close directing added
- HeadShakingHorizontal,shakeConversationBox directing added

### Removed
- UIEffect Plugin

## [0.4.0] - 2025-02-28
### Modified
- The way to compare functionType changed from "==" to "Contains()"

## [0.4.1] - 2025-03-05
### Modified
- StoryView functions changed to virtual

## [0.4.2] - 2025-03-05
### Added
- Timeline added to snapshot

## [0.4.3] - 2025-03-06
### Modified
- Methods of conversation function changed to virtual
- OnConversationStart event Action of conversation function added

## [0.4.4] - 2025-03-06
### Modified
- PlayerNameKey modified

## [0.4.5] - 2025-03-06
### Modified
- Methods of StageCharacter changed to virtual 

## [0.4.6] - 2025-03-06
### Modified
- PlayerNameKey modified
- FunctionInConversation prefix and suffix changed to {{ and }}
- CenterConversation function added
- Text Animator Data Missing resolved

## [0.4.7] - 2025-03-06
### Modified
- Zoom function added
- CharacterShakeHorizontal function added
- CharacterShakeVertical function added
- HeadShakingHorizontal -> CamShakingHorizontal function renamed
- ConversationView reset method added in UIOff function

## [0.4.8] - 2025-03-07
### Modified
- Camera reset code added in camera directing function 

## [0.4.9] - 2025-03-07
### Fix
- EpisodeIndex of TextElement always was 0 fixed   

## [0.5.0] - 2025-03-07
### Modified
- SCG function changed to use UGUI Image   

## [0.5.1] - 2025-03-07
### Added
- VignetteCircleIn,Out function added

## [0.6.0] - 2025-03-08
### Added
- CharacterObject SnapshotData in TextElement added   

## [0.6.1] - 2025-03-10
### Fix
- functionInConversation not executed fixed      

## [0.6.2] - 2025-03-10
### Modified
- Character_overlapObject default duration changed from 0.5 to 0.3
- Executing SetNativeSize() in SCG Function added 

## [0.6.3] - 2025-03-10
### Fix
- Checkpoint Snapshot bug fixed       

## [0.6.4] - 2025-03-10
### Added
- SuengAVignetteIn,Out Function added
- WipeLeftOut,WipeRightIn Function added
- ConditionComparator remove . of conditionKey automatically

## [0.7.0] - 2025-03-11
### Modified
- ECG type changed from SpriteRenderer -> Image      

## [0.7.1] - 2025-03-12
### Modified
- BackgroundView Dim Image added

## [0.7.2] - 2025-03-12
### Fixed
- StageCharacter Focus bug fixed

## [0.7.3] - 2025-03-12
### Added
- Material reset after vignette code added 

## [0.8.0] - 2025-03-13
### Removed
- TextElement Snapshot removed 

## [0.8.1] - 2025-03-17
### Modified
- Modified to ignore Function case sensitivity.
- Character Functions modified to put custom duration.

## [0.8.2] - 2025-03-21
### Modified
- UIOff Function execute DOKill()
- Position variables added to Character_show Function
- EpisodeData EpisodeIndex variable changed to EpisodeName
- ColorFilter Function added
- VignetteLoop Function added

## [0.8.3] - 2025-03-21
### Modified
- Timeline Function set ConversationView Interactable 

## [0.8.4] - 2025-03-25
### Modified
- ConditionComparator bool variable bug fixed 

## [0.8.5] - 2025-03-25
### Modified
- SCG EndFunction duration variable added 

## [0.8.6] - 2025-03-27
### Fixed
- InActiveSave bug fixed 

## [0.8.7] - 2025-03-27
### Added
- SetTypeWriter method in ConversationView added  

## [0.8.8] - 2025-04-07
### Modified
- ContentRunner changed to create all instances contained parent
- StoryContent Init method added

## [0.8.9] - 2025-04-08
### Modified
- StoryContent CurTextElementIndex be assign to index of EpisodeData.textElements

## [0.8.10] - 2025-04-15
### Add
- Bloom,Bloom YoYo Functions added

## [0.8.11] - 2025-04-16
### Add
- Functions related to ECG added variables for nativeSize

## [0.8.12] - 2025-04-18
### Add
- OverlaySCG set nativeSize of sprite

## [0.8.13] - 2025-04-23
### Modified
- Voice import debug code modified
- PlayVoice function prefix code removed(SoundManager.cs)

## [0.9.0] - 2025-04-23
### Add
- ISavePoint interface removed
- IVoiceFunction interface Added

## [0.9.1] - 2025-04-24
### Modified
- SoundManager provides Play function with AudioClip variable

## [0.9.2] - 2025-04-25
### Modified
- Conversation in TextElement for voice replaces <>{} to empty

## [0.9.3] - 2025-04-25
### Modified
- VoicePath modified to be used by multiple TextElement

## [0.9.4] - 2025-05-07
### Modified
- VoicePath debugging code modified 

## [0.9.5] - 2025-05-08
### Modified
- VoicePath code modified 

## [0.9.6] - 2025-05-08
### Modified
- VoicePath debugging code modified 

## [0.9.7] - 2025-05-09
### Added
- #if UNITY_EDITOR code added to StoryUtil.ExistVoiceFile code  

## [0.9.8] - 2025-05-12
### Fixed
- A bug ConversationView.Interactive changed incorrectly fixed   

## [0.9.9] - 2025-05-12
### Modified
- PlayVoice() of IVoiceFunction use CharacterName of GetCharacter()

## [0.9.10] - 2025-05-12
### Modified
- GetCharacter() changed to GetVoiceCharacterName() in IVoiceFunction

## [0.9.11] - 2025-05-12
### Fixed
- GetVoiceCharacterName() code error fixed

## [0.9.12] - 2025-05-13
### Fixed
- UI view navigation system OnSelect not executed bug fixed

## [0.9.13] - 2025-05-30
### Modified
- StoryContent exception code added

## [0.9.14] - 2025-06-04
### Modified
- StageCharacter default duration modified

## [0.9.15] - 2025-06-05
### Added
- Position parameter added in shakeConversationBox function 

## [0.9.16] - 2025-06-16
### Modified
- Vignette function error fixed 

## [0.9.17] - 2025-06-19
### fixed
- SoundManager BGM unexpected play fixed 

## [0.9.18] - 2025-06-19
### fixed
- SoundManager BGM unexpected play fixed 

## [0.9.19] - 2025-06-23
### modified
- Character_overlapObject in Conversation modified to allow delay

## [0.9.20] - 2025-06-25
### modified
- Overlay method in ImageDirecting modified

## [0.10.0] - 2025-07-07
### added
- TextElement uid diff merge tool

## [0.10.1] - 2025-07-08
### modified
- Resume state added in IContent

## [0.10.2] - 2025-07-08
### modified
- INavigationGroup removed

## [0.10.3] - 2025-07-09
### modified
- SelectedGameObject already selected calls OnSelect Function. 

## [0.10.4] - 2025-07-10
### modified
- Editor define code added

## [0.10.5] - 2025-07-10
### modified
- Editor define code added

## [0.10.6] - 2025-07-14
### modified
- SetFirstSelectedObject method added in View.cs

## [0.10.7] - 2025-07-16
### modified
- SetFirstSelectedObject method added in View.cs 

## [0.11.0] - 2025-07-17
### modified
- Package UI dependencies seperated to sample 

## [0.11.1] - 2025-07-17
### added
- Package Sample 

## [0.11.2] - 2025-07-17
### added
- Package Sample has been minimized.

## [0.11.3] - 2025-07-17
### added
- Package Sample has been minimized

## [0.12.0] - 2025-08-06
### added
- Story localization data added

## [0.12.1] - 2025-08-06
### fixed
- build compile error fixed

## [0.12.2] - 2025-08-07
### modified
- conversation locale method added 

## [0.13.0] - 2025-08-08
### modified
- Localization data has been replaced to conversationIndex.

## [0.13.1] - 2025-08-08
### removed
- LocaleManager.cs

## [0.13.2] - 2025-08-08
### removed
- ConversationIndex logic modified

## [0.14.0] - 2025-09-05
### Removed
- Teaching Assistant Life sample (no longer included in the package)

### Added
- **FunctionEventRegistry**
- **Episode CSV Extension**

### Changed
- **TextElement generation logic** has been updated for better consistency and maintainability
- **Function execution flow** has been revised to improve extensibility

## [0.14.1] - 2025-09-09
### modified
- StoryFunc Presenter type has changed to generic T

## [0.14.4] - 2025-09-11
### modified
- GetFunctionValue error handling code added 

## [0.14.5] - 2025-09-12
### modified
- TextElement Character parsing exception code modified 

## [0.14.6] - 2025-09-12
### modified
- TextElement Conversation parsing exception code modified 

## [0.14.7] - 2025-09-19
### modified
- Presenter bind logic has changed
- The GetFunctionValue function has been removed. You can now use TextElement.FunctionValue instead

## [0.14.8] - 2025-09-19
### modified
- FunctionValue.HasValue doesn't compare to string value

## [0.14.9] - 2025-09-22
### fix
- now StoryflowController StartTextElement changes EpisodeData too 

## [0.14.10] - 2025-09-26
### fix
- build error caused by editor code fixed

## [0.14.11] - 2025-09-29
### modified
- _StoryFlowController.enableNext default value changed from false to true

## [0.14.12] - 2025-09-30
### fixed
- Error when openGeneric function class is generated fixed  

## [0.14.13] - 2025-10-15
### modified
- Improved usability of the FunctionEventRegistry class 

## [0.14.14] - 2025-10-16
### fixed
- Fixed malfunction issue in IF function

## [0.14.15] - 2025-10-16
### fixed
- Fixed onStart variable issue in Condition function
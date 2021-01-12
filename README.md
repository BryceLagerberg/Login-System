# Login-System

making a login interface that records stats. May later be expanded on


#THINGS THAT HAVE BEEN DONE/FIXED AND UPLOADED----------------------------------------------------------------------------

1[x] Write a function to verify the connection to the SQL Server

2[x] make a new textbox for email

3[x] make a new textbox for CreatedOn

4[x] make a new textbox for LastLogin

5[x] Add SQL table creation if the database does not have the required tables within it.

6[x] Multithread SQLControl's TestConnection function to remove lag

7[x] Write a Slide function as a form animation

8[x] fix loggedIn reporting in sql table

9[X] fix lastname reporting in sql table

10[x] fix lastlogin reporting in sql table

11[x] *add a change password option

12[x] Add Password Mask / Clear Login 

13[x] *add a password visible toggle

14[x] link the profile photos to individual profiles, probably through sql table

15[x] add a toolstrip drop down button menu that we populated with a logout button and pic browser

16[x] finish populating the text boxes on ProfileWindow

17[x] create a log out button on the Profile Window Form

18[x] Add a profile picture option that saves previously used photos for quick access.

19[x] Add flare to things

20[x] *Add password encrypt / decrypt function to SQLControl

21[x] *make it so only one person can log in at a time per profile

22[x] fix bug with create account window being thrown out not hidden

23[x] *build a function that verifies username availability

24[x] make closing the window log out and close out

25[x] add an executable icon/ all the forms

26[x] make the profile window tool bars clear button actually work

27[x] Table creation is hard coded to BryceDB

28[x] Table creation is missing columns

29[x] Make an images folder for storing profile pictures on the C:\ drive

30[x] Test connection is locked at connected after a successful connection

31[x] Make server and database textboxes start blank and remember the last locally used input

32[x] fix broken accounts so u can test stuff

33[x] add a friends list in the expanded window 

34[x] make a chat box pop out window accessable from the friends list

35[x] make friends list elements use correct info per user?

36[X] make a chat system

37[X] Open Sql to wifi

38[X] change profile picture through right click instead of button

39[x] on friends list make the loggedIn update in real time

40[x] make logged in users appear green logged out users appear gray

41[x] fix chat window text counter location

42[x] Make chat window close when you log out

43[x] make it so you can login by hitting enter on the password textbox

44[x] make the correct name show on each message in chat window

45[x] come up with more efficent way to display your messages

46[x] make each chat instance have its own tab

47[x] make a CW expand button

48[x] add slide effect to CW expand button

49[x] replace login window slidedown function with new slide funtion

50[x] add slide on hover effect to CW expand button

51[x] fix friends list panel sizing

52[x] fix anchoring of expand button

53[x] make expand button autoshrink if not being hovered

54[x] add mouse move box to replace mouseEnter and mouseLeave events to fix expand button edge case 

55[x] make expand window button for profile window, make slide effects for both button and the form

56[x] make profileWindow expandable

57[x] fix sizing on loginWindow slide function

58[x] make remember me check box stay checked

59[x] add password requirements 

60[x] add check box for password requirements being met or not

61[x] add description for password and username requirements above or below their text boxes

62[x] add a remember my username checkbox to the login

63[x] make create account password textbox text hidden

64[x] make create account button only work if username and password requirements are met


#THINGS THAT NEED TO BE DONE-------------------------------------------------------------------------------------------------------------

[ ] *add a change username option?

[ ] make an applications tab with cool tools like web browser and calculator etc

[ ] *make good flair, remove bad flair
	[ ] make a safe password checkbox that if checked gives a message saying thats a bad idea and then doesnt remember the password

[ ] *add a drag and drop to the profile pictures so you can change easier

[ ] make the friends list live update and organize by online/offline status

[ ] make the server and database selection a pop out window accessable by a settings symbol or something

[ ] create sql stored procedures

[ ] Make chat window appear to the right of the profile window

[ ] remove the logged in user from their own friends list

[ ] add on double click events for all friends list controls

[ ] make expand arrow change directions after completing the grow or shrink animation

[ ] fix transperency issue with expand button

[ ] change message cap to a hard limit instead of replacing old text in chat window

[ ] fix tab order on login window

BUGS TO FIX------------------------------------------------------------------------------------------------------------------------------

[ ] fix expand button jumping to the top (havent been able to recreate this bug yet)

[ ] fix bug that says wrong username/password when logging in to an already logged in account

[ ] profile window should reset to original size on user log out or when the user closes

[ ] all chat messages are outputing the same message regardless of input when logged in as bobby and sending to bryce, but works fine for other users bobby sends a message to.

[ ] after a failed login the login window name always stays as 'Bad Login' it should change back to login window after a successful login, or when error message is closed.

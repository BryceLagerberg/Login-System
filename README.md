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

65[x] add a second project to my solution in the form of a budget tracker

66[x]  profile window should reset to original size on user log out or when the user closes

67[x] make expand arrow change directions after being clicked so it points in the direction that if clicked the page grow or shrink towards

68[x] fix transperency issue with expand button

69[x] make the listbox items easy to distinguish between gains and losses

70[x] add a third project to the solution to make back and forth information passing possible

71[x] make an applications tab for tools like web browser and calculator etc

72[x] fix tab order on login window

73[x] on the budget tracker page add a second tab to the listbox section that will hold a graph (probably a pie graph)

74[x] make a new sql table to hold budget tracker data

75[x] connect the budget tracker listbox of transactions to the new budget tracker sql table

76[x] name the tab pages for the listbox and graph inside budget tracker

77[x] add a third tab to the budget tracker to hold all account transactions(in the same location as the current transactions are displayed)

78[x] move login system functions and sql control over to the utilities project

79[x] make it if you dont pick a catagory while adding a gain or loss it wont let you post the transaction and gives u a message to select a catagory first

80[x] clear the fields after posting transaction

81[x] add transaction date to the transactions list items

82[x] only transactional gains are being displayed on the all transactions tab when it should also show expense transactions

83[x] daily and weekly expenses are not being calculated for some reason in the earnings statement

#THINGS THAT NEED TO BE DONE-------------------------------------------------------------------------------------------------------------

a[ ] *add a change username option?

c[ ] make good flair, remove bad flair
	[ ] *make a save password checkbox that if checked gives a message saying thats a bad idea and then doesnt remember the password

d[ ] *add a drag and drop to the profile pictures so you can change easier

e[ ] make the friends list live update and organize by online/offline status

f[ ] *make the server and database selection a pop out window accessable by a settings symbol or something

g[ ] create sql stored procedures

h[ ] Make chat window appear to the right of the profile window

i[ ] remove the logged in user from their own friends list

j[ ] add on double click events for all friends list controls

k[ ] *change message cap to a hard limit instead of replacing old text in chat window

l[ ] make the displayed gains and expenses change depending on the date selected from on the calender

m[ ] populate the earnings statement inside the budget tracker

n[ ] make it so that the budget tracker application is hidden on close not deleted and recreated when reopened(should still be deleted on user log out)

o[ ] get rid of the utilities folders on both projects and turn the utilities project into the now deleted utilites folder.

p[ ] in budget tracker replace the numericupdown with something that goes above 100 so its possible to post transactions above 100 dollars

q[ ] populate the budget tracker pie graph with data from the sql database

r[ ] Inside of utilities.Functions.LoadSettings there is a reference to Login System.resources that is out of bounds, currently its commented out but needs to be fixed for real at some point

s[ ] change the all transactions tab to show transactions in order by date (currently they are displayed in an unreliable way that can change randomly)

t[ ] *add a post number to the budget tracker sql table to make tracking transactions easier (not sure i actually need this anymore)

u

v

w

x

y

z

* still undecided if i actaully want to do this or not
#BUGS TO FIX------------------------------------------------------------------------------------------------------------------------------

[ ] fix expand button jumping to the top (havent been able to recreate this bug yet)

[ ] fix bug that says wrong username/password when logging in to an already logged in account

[ ] all chat messages are outputing the same message regardless of input when logged in as bobby and sending to bryce, but works fine for other users bobby sends a message to. (havent been able to recreate this bug)

[ ] after a failed login the login window name always stays as 'Bad Login' it should change back to login window after a successful login, or when error message is closed.

[ ] if you login with the enter key the computer makes a ping sound. doesnt happen if the login button is pressed though

[ ] in all transactions tab the amount part of the expense transactions show a positive value instead of negative as an expense should

[ ] the earnings statement is miscalculating the daily earnings somehow
#IDEAS ON HOW TO COMPLETE ITEMS ON THE TO DO LIST--------------------------------------------------------------------------------------------

currently making the loadtransactions function in form1, it will be very similar to the loadchat function in chatwindow

#QUESTIONS I HAVE ABOUT HOW TO DO THINGS-----------------------------------------------------------------------------------------------------



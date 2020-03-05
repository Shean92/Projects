# Projects
My official repository to hold my projects I want to share

Http Server Status Program
I made this program for Brigham Young University of Idaho. These programs helped the employee in charge of correcting, deleting and reorganizing files. I had never used ".net" before and this was a wonderful experience for me to learn how it all works. I finished these programs over the course of 2 weeks or 60 hours of total work time. I learned how http requests are made, I learned how to use exceptions to my advantage, and I learned valuable search techniques to find the help that I needed. This experience gave me confidence to trust in my own work and to realize that I am capable of doing anything I set my mind to. I started these programs from scratch without any supervision of my code. There were no other students or employees who could help me with my code or correct me since I am the only software engineering student in my department. I learned from trial and error which I believe improved my ability to trust myself and to find out how to solve problems on my own. There were times when I became frustrated, but I never gave up and the sense of progress and accomplishment that each step gave me kept me moving forward. I continued looking for optimization techniques and better ways to accomplish my task. This experience was one of my most favorite and challenging experiences so far.

The reason I separated these two files was because I did not want to convolute the Collect program with the Server Tester program. I often think about testing the status of each of the files from the Collect file directly after I pull them, but there are some complications. We want to keep the name of the file path, and in order to test the file's server status we needed to change the name of the file to be a URL temporarily. I felt it was too complicated to do all of this in one program and I felt that since they do separate tasks they should stay separate.
{
  Collect
  Collect and Server Tester are two folders that work together. In the Collect program I search a specified folder for all the files in 
  each sub folder and I pull out the information associated with those files, such as: owner, date created, date modified, size of file, 
  etc.

  Server Tester
  The Server Tester folder works with the Collect folder. After I run the collect program and recieve all the data from the files, I run 
  the Server Tester program. In the Server Tester program I take the names of all the files and run them in an http request. I recieve the 
  status code from each of the files and I make a new file with this information and I include the information from Collect.
}

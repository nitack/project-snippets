This user story was my favorite one to work on because of the lesson it taught.  The problem was that when a modal launched to approve or deny a time off request the title always said Approve Note.  I had to make the title show "Denial Note" if the request was not bing approved.  

For a few months I had been working exclusively in C#.  If you've ever heard of Maslow's hammer ("if all you have is a hammer, everything looks like a nail"), you'll know where this leads.  I spent hours trying to change a modal title using C# Razor markup.  I was trying to use local variables that changed when existing JavaScript executed.  After numerous trial and error attempts spanning many hours I got frustrated and took a break.  When I walked back to the problem a few hours later something dawned on me.  I was trying to have JavaScript execute a local variable change using Razor Markup, but this kind of element change was already very easy to do using JavaScript.  I removed dozens of line of code that I had written in razor and wrote two simple lines of JavaScript:

	document.getElementById("noteModalTitle").innerHTML = "Approval Note";
	document.getElementById("noteModalTitle").innerHTML = "Denial Note";

The lesson: different tools are best suited for different jobs and we shouldn't get so comfortable with one tool that we ignore the rest.

Skills: JavaScript, C#, Razor, HTML, CSS, Bootstrap
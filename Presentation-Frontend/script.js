document.addEventListener("DOMContentLoaded", () => {
  const apiUrl = "/api"; // Use relative path since backend serves the frontend

  // State
  let jwtToken = localStorage.getItem("jwtToken") || "";
  let ideaSessionId = "";
  let currentStepId = "";

  // DOM Elements
  const authSection = document.getElementById("auth-section");
  const ideaSection = document.getElementById("idea-section");
  const questionsSection = document.getElementById("questions-section");
  const loadingSection = document.getElementById("loading-section");
  const resultSection = document.getElementById("result-section");
  const completedSection = document.getElementById("completed-section");
  const viewPlanBtn = document.getElementById("view-plan-btn");

  const jwtTokenInput = document.getElementById("jwt-token");
  const saveTokenBtn = document.getElementById("save-token-btn");
  const ideaTitleInput = document.getElementById("idea-title");
  const createIdeaBtn = document.getElementById("create-idea-btn");
  const questionTitle = document.getElementById("question-title");
  const questionText = document.getElementById("question-text");
  const questionAnswerInput = document.getElementById("question-answer");
  const submitAnswerBtn = document.getElementById("submit-answer-btn");
  const mvpPlanContainer = document.getElementById("mvp-plan");

  // --- INITIALIZATION ---
  function initialize() {
    if (jwtToken) {
      jwtTokenInput.value = jwtToken;
      showSection("idea");
    } else {
      showSection("auth");
    }
  }

  // --- UI HELPERS ---
  function showSection(section) {
    [
      authSection,
      ideaSection,
      questionsSection,
      loadingSection,
      resultSection,
      completedSection,
    ].forEach((s) => s.classList.add("hidden"));
    document.getElementById(`${section}-section`).classList.remove("hidden");
  }

  function showAlert(message, isError = true) {
    alert(isError ? `Error: ${message}` : message);
  }

  // --- API HELPERS ---
  async function apiFetch(endpoint, method = "GET", body = null) {
    const headers = {
      "Content-Type": "application/json",
      Authorization: `Bearer ${jwtToken}`,
    };

    const config = {
      method,
      headers,
    };

    if (body) {
      config.body = JSON.stringify(body);
    }

    try {
      const response = await fetch(`${apiUrl}${endpoint}`, config);
      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(
          errorData.error || `Request failed with status ${response.status}`
        );
      }
      // Handle cases with no content
      const contentType = response.headers.get("content-type");
      if (contentType && contentType.indexOf("application/json") !== -1) {
        return await response.json();
      } else {
        return { success: true };
      }
    } catch (error) {
      showAlert(error.message);
      throw error;
    }
  }

  // --- EVENT HANDLERS ---
  saveTokenBtn.addEventListener("click", () => {
    const token = jwtTokenInput.value.trim();
    if (token) {
      jwtToken = token;
      localStorage.setItem("jwtToken", token);
      showAlert("Token saved successfully!", false);
      showSection("idea");
    } else {
      showAlert("Please paste a valid JWT token.");
    }
  });

  createIdeaBtn.addEventListener("click", async () => {
    const title = ideaTitleInput.value.trim();
    if (!title) {
      showAlert("Please enter a title for your idea.");
      return;
    }

    try {
      const response = await apiFetch(
        "/IdeaSessions/CreateIdeaSession",
        "POST",
        { title }
      );
      console.log("Response from CreateIdeaSession:", response); // CRITICAL DEBUGGING
      // The endpoint returns the session object directly, not wrapped in { success, data }.
      if (response && response.id) {
        console.log(
          "CreateIdeaSession was successful, fetching next question."
        );
        ideaSessionId = response.id;
        await fetchNextQuestion();
      } else {
        console.error(
          "CreateIdeaSession failed or did not return a valid session object.",
          response
        );
        showAlert("Could not create session. Check the console for details.");
      }
    } catch (error) {
      console.error("Failed to create idea session:", error);
    }
  });

  submitAnswerBtn.addEventListener("click", async () => {
    const answer = questionAnswerInput.value.trim();
    if (!answer) {
      showAlert("Please provide an answer.");
      return;
    }

    try {
      // Use POST to /api/Step as per Swagger
      await apiFetch("/Step", "POST", {
        ideaSessionId: ideaSessionId,
        stepTemplateId: currentStepId, // The ID from next-question is the StepTemplateId
        userInput: answer,
      });
      questionAnswerInput.value = "";
      await fetchNextQuestion();
    } catch (error) {
      console.error("Failed to submit answer:", error);
    }
  });

  // --- CORE LOGIC ---
  async function fetchNextQuestion() {
    try {
      console.log(`Fetching next question for session ID: ${ideaSessionId}`);
      const response = await apiFetch(
        `/IdeaSessions/${ideaSessionId}/next-question`
      );
      console.log("Response from next-question:", response); // DEBUGGING

      if (response.allCompleted) {
        // Backend now handles generation automatically.
        // We need a new way to fetch and display the result.
        console.log("All questions completed. Backend is generating the plan.");
        showSection("completed"); // Show the completed section with the "View Plan" button
      } else {
        console.log("Displaying next question:", response);
        currentStepId = response.stepTemplateId; // Correctly use stepTemplateId
        questionTitle.textContent = response.title; // Use the 'title' field from the response
        questionText.textContent = response.question;
        showSection("questions");
      }
    } catch (error) {
      console.error("Failed to fetch next question:", error);
    }
  }

  viewPlanBtn.addEventListener("click", async () => {
    showSection("loading");
    try {
      // This endpoint needs to be created in the backend
      const response = await apiFetch(`/IdeaSessions/${ideaSessionId}/plan`);
      if (response && response.id) {
        displayMvpPlan(response);
      } else {
        showAlert("The plan is not ready yet or could not be found.");
        showSection("completed");
      }
    } catch (error) {
      console.error("Failed to fetch MVP plan:", error);
      showSection("completed");
    }
  });

  function displayMvpPlan(plan) {
    const p = (value) => value || "Not specified";
    const p_replace = (value) =>
      (value || "Not specified").replace(/\n/g, "<br>");

    let html = `
            <h3>${p(plan.title)}</h3>
            <p><strong>Goal:</strong> ${p(plan.goal)}</p>
            <p><strong>Experience Level:</strong> ${p(plan.experienceLevel)}</p>
            <h4>Problem Statement</h4>
            <p>${p(plan.problemStatement)}</p>
            <h4>Solution Approach</h4>
            <p>${p(plan.solutionApproach)}</p>
            <h4>Value Proposition</h4>
            <p>${p(plan.valueProposition)}</p>
            <h4>Target Audience</h4>
            <p><strong>Primary:</strong> ${p(plan.primaryTargetAudience)}</p>
            <p><strong>Secondary:</strong> ${p(
              plan.secondaryTargetAudience
            )}</p>
            <h4>Key Features</h4>
            <p>${p_replace(plan.keyFeatures)}</p>
            <h4>Technical Stack</h4>
            <p>${p(plan.technicalStack)}</p>
            <h4>Business Plan</h4>
            <p><strong>Estimated Budget:</strong> ${p(plan.estimatedBudget)}</p>
            <p><strong>Timeline:</strong> ${p(plan.timelineEstimate)}</p>
            <p><strong>Monetization:</strong> ${p(
              plan.monetizationStrategy
            )}</p>
            <h4>Next Steps</h4>
            <p>${p_replace(plan.nextSteps)}</p>
        `;
    mvpPlanContainer.innerHTML = html;
    showSection("result");
  }

  // --- START THE APP ---
  initialize();
});

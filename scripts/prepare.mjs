import { execSync } from "child_process";

function run(cmd) {
  try { execSync(cmd, { stdio: "inherit" }); } catch {}
}

run("husky");
run("git submodule update --init --remote --recursive");
run("bash topcon-ai-core/loader.sh");
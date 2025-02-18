import { useEffect, useState } from "react";

const decodeBase64Url = (input) => {
  let base64 = input.replace(/-/g, "+").replace(/_/g, "/");
  while (base64.length % 4 !== 0) {
    base64 += "=";
  }
  return window.atob(base64);
};

export function useAuth() {
  const [user, setUser] = useState(() => {
    const token = localStorage.getItem("token");
    if (token) {
      const payload = JSON.parse(decodeBase64Url(token.split(".")[1]));
      return { isAdmin: payload.IsAdmin === "True" };
    }
    return null;
  });

  useEffect(() => {
    const handleStorageChange = () => {
      const token = localStorage.getItem("token");
      if (token) {
        const payload = JSON.parse(decodeBase64Url(token.split(".")[1]));
        setUser({ isAdmin: payload.IsAdmin === "True" });
      } else {
        setUser(null);
      }
    };

    window.addEventListener("storage", handleStorageChange);
    return () => window.removeEventListener("storage", handleStorageChange);
  }, []);

  return user;
}

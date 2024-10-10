export const errorKeys = (obj: { [key: string]: string[] }): string[] => {
  const result: string[] = [];

  for (const key in obj) {
    if (obj.hasOwnProperty(key)) {
      const values = obj[key];
      result.push(...values);
    }
  }

  return result;
};

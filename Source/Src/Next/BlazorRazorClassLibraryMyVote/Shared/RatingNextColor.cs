using PublicCommon;
using MudBlazor;
using static MudBlazor.CategoryTypes;
namespace BlazorRazorClassLibraryMyVote.Shared;

public static class RatingNextColor
    {
    public static Color GetColor(this RatingEnum? rating)
        {
        return rating switch
            {
                RatingEnum.VeryBad => Color.Error, //"red", but showing as pink
                RatingEnum.Bad => Color.Warning,   //"yellow",
                RatingEnum.OkOk => Color.Secondary,//pink
                RatingEnum.GoodWork => Color.Tertiary,//green light
                RatingEnum.GreatWork => Color.Success, //green
                //_ => Color.Info //blue color
                _ => Color.Default
                };
        //red -> 
        //expected is as below but its not available in mudblazor so this alternative
        //Red -> Orange -> Yellow -> Light Green -> Green
        }
    public static Color GetColor(this int? ratingInt) => (ratingInt ?? 0).ParseToEnum<RatingEnum>().GetColor();

    public static Color GetColor(this sbyte? ratingSbyte) => ((int?)ratingSbyte).GetColor();
    public static Color GetColor(this sbyte ratingSbyte) => GetColor(ratingSbyte);

    public static string GetColorAsString(this RatingEnum? rating)
        {
        return GetColor(rating).ToString().ToLower();
        }
    public static string GetColorAsString(this sbyte? ratingSbyte) => GetColor(ratingSbyte).ToString().ToLower();
    public static string GetColorAsString1(this sbyte ratingSbyte) => GetColorAsString(ratingSbyte);



    public static string GetColorAsCodeForBackground(this RatingEnum? rating)
        {
        return rating switch
            {
                RatingEnum.VeryBad => Colors.Red.Lighten1, //"#EF5350",//Red.Light1   //Color.Error, //"red", but showing as pink
                RatingEnum.Bad => Colors.Yellow.Lighten2,//Color.Warning,   //"yellow",
                RatingEnum.OkOk => Colors.Pink.Lighten2,//Color.Secondary,//pink
                RatingEnum.GoodWork => Colors.Green.Lighten1,//Color.Tertiary,//green light
                RatingEnum.GreatWork => Colors.LightGreen.Accent3,//Color.Success, //green
                //_ => Color.Info //blue color
                _ => Colors.BlueGrey.Lighten5//white
                };
        //red -> 
        //expected is as below but its not available in mudblazor so this alternative
        //Red -> Orange -> Yellow -> Light Green -> Green
        }
    public static string GetColorAsCodeForBackground(this sbyte? ratingSbyte) => ((int?)ratingSbyte ?? 0).ParseToEnum<RatingEnum>().GetColorAsCodeForBackground();




    }
